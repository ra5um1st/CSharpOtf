using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using OpenGL.TextDrawing.Cff;
using OpenGL.TextDrawing.Cff.Type2Charstring;
using OpenGL.TextDrawing.OpenType;

namespace OpenGL.TextDrawing
{
    public partial class OpenTypeParser
    {
        public List<T2ProgramContext> Charstrings { get; set; }

        public void Parse(string path)
        {
            var bytes = File.ReadAllBytes(path);
            var offset = 0;

            var tableDirectory = StructByteReader<TableDirectory>.Read(bytes, ref offset);

            var cmapOffset = Convert.ToInt32(tableDirectory.TableRecords.First(item => item.Tag == "cmap").Offset);
            var cmapOffset1 = cmapOffset;
            var cmap = StructByteReader<Cmap>.Read(bytes, ref cmapOffset);
            var format4Table = cmap.EncodingRecords[2].GetFormat4Subtable(bytes, ref cmapOffset1);

            var cffOffset = Convert.ToInt32(tableDirectory.TableRecords.First(item => item.Tag == "CFF").Offset);
            var cffOffset1 = cffOffset;

            var cff = new Cff.Cff()
            {
                Offset = cffOffset,
            };

            cff = StructByteReader<Cff.Cff>.Read(bytes, ref cffOffset1, cff);

            var privateSize = cff.TopDictIndex.Data.Private.Value.PrivateDictSize;
            var privateOffset = cffOffset + cff.TopDictIndex.Data.Private.Value.PrivateDictOffset;
            var privateOffset1 = privateOffset;

            var privateDict = new PrivateDict
            {
                Size = privateSize
            };

            privateDict = StructByteReader<PrivateDict>.Read(bytes, ref privateOffset1, privateDict);
            cff.PrivateDict = privateDict;

            var localSubrIndexOffset = privateOffset + (CffByte)privateDict.Data[CffOperator1.Subrs][0];
            var localSubrIndex = StructByteReader<LocalSubrIndex>.Read(bytes, ref localSubrIndexOffset);

            var localSubrBias = cff.GetSubrBias(CffSubrIndexType.LocalSubrIndex, localSubrIndex);
            var globalSubrBias = cff.GetSubrBias(CffSubrIndexType.GlobalSubrIndex, localSubrIndex);

            var localSubroutines = localSubrIndex.Data.Select(item => new T2Subroutine(new T2Reader(item))).ToList();
            var globalSubroutines = cff.GlobalSubrIndex.Data.Select(item => new T2Subroutine(new T2Reader(item))).ToList();

            Charstrings = new List<T2ProgramContext>();
            for (int i = 0; i < cff.CharStringsIndex.Data.Length; i++)
            {
                var context = new T2ProgramContext(cff, localSubrBias, globalSubrBias, i)
                {
                    LocalSubroutines = localSubroutines,
                    GlobalSubroutines = globalSubroutines
                };

                new T2Subroutine(new T2Reader(cff.CharStringsIndex.Data[i])).Execute(context);

                Charstrings.Add(context);
            }
        }
    }
}
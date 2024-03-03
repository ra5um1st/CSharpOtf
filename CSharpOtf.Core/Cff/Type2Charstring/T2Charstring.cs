using System;
using System.Collections.Generic;
using System.Linq;
using OpenGL.TextDrawing.Cff.Type2Charstring.Operators;

namespace OpenGL.TextDrawing.Cff.Type2Charstring
{
    public class T2CharstringBuilder
    {
        public T2Point? Witdh { get; set; }
        public T2SubpathGroup1 SubpathGroup1 { get; }
        public T2SubpathGroup2 SubpathGroup2 { get; }
        public Endchar Endchar { get; }

        //public IT2SubpathGroup1Builder SetWidth(T2Float width)
        //{

        //}

    }

    //public interface IT2SubpathGroup1Builder
    //{
    //    IT2SubpathGroup1Builder AddHStem(HStem hStem);
    //    IT2SubpathGroup1Builder AddHStem(HStemhm hStemhm);
    //    IT2SubpathGroup1Builder AddVStem(VStem vStem);
    //    IT2SubpathGroup1Builder AddVStem(VStemhm vStemhm);
    //    IT2SubpathGroup1Builder AddCntrMask(CntrMask cntrMask);
    //    IT2SubpathGroup1Builder AddHintMask(HintMask hintMask);

    //    IT2SubpathGroup2Builder Build();
    //}

    public interface IPath
    {
        IReadOnlyList<T2Point> GetPoints(int accuracy);
    }

    public interface ISegment
    {
        T2Point StartPoint { get; }
        T2Point EndPoint { get; }
    }

    public interface IPathSegment : IPath, ISegment
    {
    }

    public class T2Path : IPathSegment
    {
        private List<IPathSegment> _pathSegments = new(10);
        public IReadOnlyList<IPathSegment> PathSegments => _pathSegments;

        public T2Point StartPoint => _pathSegments.FirstOrDefault()?.StartPoint ?? new T2Point();
        public T2Point EndPoint => _pathSegments.LastOrDefault()?.EndPoint ?? new T2Point();

        public void AddPathSegment(IPathSegment segment) 
        {
            // Validate start point that must be equal previous path segment end point
            // Then add new path segment

            //if (prevSegment == null)
            //{
            //    StartPoint = segment.StartPoint;
            //}
            //else
            //{
            //    if (prevSegment.EndPoint != segment.StartPoint)
            //    {
            //        throw new Exception("The end point of previous segment must be equal to the start point of the new segment.");
            //    }
            //}

            _pathSegments.Add(segment);
        }

        public void AddPathSegments(IEnumerable<IPathSegment> pathSegments)
        {
            foreach (var pathSegment in pathSegments)
            {
                AddPathSegment(pathSegment);
            }
        }

        public IReadOnlyList<T2Point> GetPoints(int accuracy)
        {
            var result = new List<T2Point>(100);

            foreach (var pathSegment in PathSegments)
            {
                result.AddRange(pathSegment.GetPoints(accuracy));
            }

            return result;
        }

        public bool Close()
        {
            if (!_pathSegments.Any())
            {
                return false;
            }

            var firstPathSegment = _pathSegments.First();
            var lastPathSegment = _pathSegments.Last();

            if (firstPathSegment.StartPoint == lastPathSegment.EndPoint)
            {
                return true;
            }

            //TODO : сделать создание пути в билдере чарстринга 

            var line = new T2LineSegment(lastPathSegment.EndPoint, firstPathSegment.StartPoint);

            _pathSegments.Add(line);

            return true;
        }
    }
}

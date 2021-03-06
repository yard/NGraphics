﻿using System.Collections;
using System.Collections.Generic;

namespace NGraphics.Custom.Models.Segments
{
    public sealed class SvgPathSegmentList : IList<SvgPathSegment>
    {
        public SvgPathSegmentList()
        {
            _segments = new List<SvgPathSegment>();
        }

        private readonly List<SvgPathSegment> _segments;

        public SvgPathSegment Last
        {
            get { return _segments[_segments.Count - 1]; }
        }

        public int IndexOf(SvgPathSegment item)
        {
            return _segments.IndexOf(item);
        }

        public void Insert(int index, SvgPathSegment item)
        {
            _segments.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _segments.RemoveAt(index);
        }

        public SvgPathSegment this[int index]
        {
            get { return _segments[index]; }
            set { _segments[index] = value; }
        }

        public void Add(SvgPathSegment item)
        {
            _segments.Add(item);
        }

        public void Clear()
        {
            _segments.Clear();
        }

        public bool Contains(SvgPathSegment item)
        {
            return _segments.Contains(item);
        }

        public void CopyTo(SvgPathSegment[] array, int arrayIndex)
        {
            _segments.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _segments.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(SvgPathSegment item)
        {
            var removed = _segments.Remove(item);

            return removed;
        }

        public IEnumerator<SvgPathSegment> GetEnumerator()
        {
            return _segments.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _segments.GetEnumerator();
        }
    }
}
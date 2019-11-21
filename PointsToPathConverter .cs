    //[ValueConversion(typeof(Point[]), typeof(Geometry))]
    public class PointsToPathConverter //: IValueConverter
    {
        #region Interface

        public static PathGeometry Convert(IEnumerable<Point> points)
        {
            var pointsArray = points.ToArray();

            if (pointsArray.Length <= 0) { return null; }

            var start = pointsArray[0];
            var segments = new List<LineSegment>();
            for (int i = 1; i < pointsArray.Length; i++)
            {
                segments.Add(new LineSegment(pointsArray[i], true));
            }
            var figure = new PathFigure(start, segments, false); //true if closed
            var geometry = new PathGeometry();
            geometry.Figures.Add(figure);
            return geometry;

            //if (points.Length > 0)
            //{
            //    Point start = points[0];
            //    List<LineSegment> segments = new List<LineSegment>();
            //    for (int i = 1; i < points.Length; i++)
            //    {
            //        segments.Add(new LineSegment(points[i], true));
            //    }
            //    PathFigure figure = new PathFigure(start, segments, false); //true if closed
            //    PathGeometry geometry = new PathGeometry();
            //    geometry.Figures.Add(figure);
            //    return geometry;
            //}
            //else
            //{
            //    return null;
            //}
        }

        #endregion
    }
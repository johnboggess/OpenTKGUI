using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

using OpenTKGUI.Enums;
namespace OpenTKGUI.GUIElements
{
    public class VerticalGrid : GUIContainer
    {
        List<Row> _rows = new List<Row>();

        float totalRatio = 0;
        float takenHeight = 0;

        public VerticalGrid()
        {
            notifyWhenDoneSizing = true;
        }

        public void AddChild(GUIElement element, Units rowUnits, float rowHeight)
        {
            Row r = new Row(this, rowUnits, rowHeight);
            _rows.Add(r);
            r._GUIElement._ForceAddChild(element);
        }

        public override void AddChild(GUIElement element, params object[] args)
        {
            AddChild(element, args[0], args[1], args[2]);
        }

        internal override Rectangle _CalculateChildSize()
        {
            float minX = float.MaxValue;
            float maxX = float.MinValue;
            float minY = float.MaxValue;
            float maxY = float.MinValue;

            totalRatio = 0;
            takenHeight = 0;

            foreach (Row row in _rows)
            {
                if (row.Units == Units.Pixels)
                    row._GUIElement.Size = new Vector2(Size.X, row.Height);
                else if (row.Units == Units.Auto)
                    row._GUIElement.Size = new Vector2(Size.X, - 1);
                else if (row.Units == Units.Ratio)
                {
                    row._GUIElement.Size = new Vector2(Size.X, -1);
                    totalRatio += row.Height;
                }

                row._GUIElement._CalculateSize();

                Rectangle outerRect = row._GUIElement.OuterRect;

                minX = MathF.Min(minX, outerRect.Left);
                maxX = MathF.Max(maxX, outerRect.Right);
                minY = MathF.Min(minY, outerRect.Bottom + takenHeight);
                maxY = MathF.Max(maxY, outerRect.Top + takenHeight);

                if (row.Units != Units.Ratio)
                    takenHeight += row._GUIElement.RenderSize.Y;
            }

            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        internal override void _CalculateChildPositions()
        {
            for (int i = 0; i < _rows.Count; i++)
            {
                Row row = _rows[i];
                row._GUIElement._CalculatePosition();
                if (i != 0)
                {
                    Row prev = _rows[i - 1];
                    row._GUIElement._LocalPosition = new Vector2(row._GUIElement._LocalPosition.X, prev._GUIElement.OuterRect.Bottom - row._GUIElement.OuterRect.Size.Y);
                }
            }
        }

        internal override void _DoneSizing()
        {
            float remainingHeight = RenderSize.Y - takenHeight;
            if (remainingHeight <= 0)
                return;

            foreach (Row row in _rows)
            {
                if (row.Units == Units.Ratio)
                {
                    row._GUIElement.RenderSize = new Vector2(row._GUIElement.RenderSize.X, (row.Height / totalRatio) * remainingHeight);
                }
            }
        }

        public enum Units
        {
            Pixels,
            Ratio,
            Auto
        }

        public class Row
        {
            public Units Units;
            public float Height;

            internal Frame _GUIElement;

            public Row(VerticalGrid grid, Units units, float width)
            {
                _GUIElement = new Frame();
                //_GUIElement.HorizontalAlignment = HorizontalAlignment.Stretch;
                _GUIElement.BorderSize = 1;
                grid._ForceAddChild(_GUIElement);

                Units = units;
                Height = width;
            }
        }
    }
}

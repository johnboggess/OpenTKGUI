using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

using OpenTKGUI.Enums;
namespace OpenTKGUI.GUIElements
{
    public class HorizontalGrid : GUIContainer
    {
        List<Column> _columns = new List<Column>();

        float totalRatio = 0;
        float takenWidth = 0;

        public HorizontalGrid()
        {
            notifyWhenDoneSizing = true;
        }

        public void AddChild(GUIElement element, Units columnUnits, float columnWidth)
        {
            Column c = new Column(this, columnUnits, columnWidth);
            _columns.Add(c);
            c._GUIElement._ForceAddChild(element);
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
            takenWidth = 0;

            foreach (Column column in _columns)
            {
                if (column.Units == Units.Pixels)
                    column._GUIElement.Size = new Vector2(column.Width, Size.Y);
                else if (column.Units == Units.Auto)
                    column._GUIElement.Size = new Vector2(-1, Size.Y);
                else if (column.Units == Units.Ratio)
                {
                    column._GUIElement.Size = new Vector2(-1, Size.Y);
                    totalRatio += column.Width;
                }

                column._GUIElement._CalculateSize();

                Rectangle outerRect = column._GUIElement.OuterRect;

                minX = MathF.Min(minX, outerRect.Left + takenWidth);
                maxX = MathF.Max(maxX, outerRect.Right + takenWidth);
                minY = MathF.Min(minY, outerRect.Bottom);
                maxY = MathF.Max(maxY, outerRect.Top);

                if (column.Units != Units.Ratio)
                    takenWidth += column._GUIElement.RenderSize.X;
            }

            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        internal override void _CalculateChildPositions()
        {
            for (int i = 0; i < _columns.Count; i++)
            {
                Column column = _columns[i];
                column._GUIElement._CalculatePosition();
                if (i != 0)
                {
                    Column prev = _columns[i - 1];
                    column._GUIElement._LocalPosition = new Vector2(prev._GUIElement.OuterRect.Right, column._GUIElement._LocalPosition.Y);
                }
            }
        }

        internal override void _DoneSizing()
        {
            float remainingWidth = RenderSize.X - takenWidth;
            if (remainingWidth <= 0)
                return;

            foreach (Column column in _columns)
            {
                if (column.Units == Units.Ratio)
                {
                    column._GUIElement.RenderSize = new Vector2((column.Width / totalRatio) * remainingWidth, column._GUIElement.RenderSize.Y);
                }
            }
        }

        public class Column
        {
            public Units Units;
            public float Width;

            internal Frame _GUIElement;

            public Column(HorizontalGrid grid, Units units, float width)
            {
                _GUIElement = new Frame();
                _GUIElement.VerticalAlignment = VerticalAlignment.Stretch;
                _GUIElement.BorderSize = 1;
                grid._ForceAddChild(_GUIElement);

                Units = units;
                Width = width;
            }
        }
    }
}

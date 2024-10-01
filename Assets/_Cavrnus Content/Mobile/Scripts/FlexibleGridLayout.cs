using UnityEngine;
using UnityEngine.UI;
using Application = UnityEngine.Device.Application;

namespace Cavrnus.UI
{
    public class FlexibleGridLayout : GridLayoutGroup
    {
        public int PortraitColumnCount = 1;
        public int LandscapeColumnCount = 2;
        
        [System.Serializable]
        public class BreakPoint
        {
            public float AspectRatio;
            public Vector2 TargetRowsColumns;
        }

        public RectTransform TargetRectWithProperWidth;

        [Tooltip("This only applies to phone platforms")]
        public bool UseAspectRatios = false;
        public BreakPoint[] BreakPoints;
        public Vector2 DefaultBreakPoint = Vector2.one * 2;
        
        public enum CellSizeConstraintEnum
        {
            Vertical, Horizontal, None
        }

        [Space]
        public CellSizeConstraintEnum CellSizeConstraint;
        
        [Space]
        public int ColumnCount = 4;
        public int RowCount = 2;

        protected override void Start()
        {
            base.Start();

            if (!Application.isPlaying)
                return;
        }

        public override void SetLayoutHorizontal()
        {
            UpdateCellSize();
            base.SetLayoutHorizontal();
        }
 
        public override void SetLayoutVertical()
        {
            UpdateCellSize();
            base.SetLayoutVertical();
        }
 
        private void UpdateCellSize()
        {
            if (ColumnCount <= 0 || RowCount <= 0) 
                return;

            var rt = TargetRectWithProperWidth != null ? TargetRectWithProperWidth : rectTransform;
            
            var x = (rt.rect.size.x - padding.horizontal - spacing.x * (ColumnCount - 1)) / ColumnCount;
            var y = (rt.rect.size.y - padding.vertical - spacing.y * (RowCount - 1)) / RowCount;
            
            constraint = Constraint.FixedColumnCount;
            constraintCount = ColumnCount;

            switch (CellSizeConstraint) {
                case CellSizeConstraintEnum.Vertical :
                    cellSize = new Vector2(x, cellSize.y);    
                    break;
                case CellSizeConstraintEnum.Horizontal :
                    cellSize = new Vector2(cellSize.x, y);    
                    break;
                case CellSizeConstraintEnum.None : 
                    cellSize = new Vector2(x, y);
                    break;
            }
        }
    }
}
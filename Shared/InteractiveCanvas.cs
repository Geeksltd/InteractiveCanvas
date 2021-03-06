﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zebble;
using Olive;

namespace Zebble.Plugin
{
    public partial class InteractiveCanvas : Zebble.Canvas
    {
        const int COMPLETE_CIRCLE_DEGREE = 360;

        AsyncLock AsyncLock = new AsyncLock();

        public bool CanRotateX { get; set; } = true;
        public bool CanRotateY { get; set; } = true;
        public bool CanRotateZ { get; set; } = true;
        public bool CanDragX { get; set; } = true;
        public bool CanDragY { get; set; } = true;
        public bool CanScaleX { get; set; } = true;
        public bool CanScaleY { get; set; } = true;
        public float MaxRotateX { get; set; } = COMPLETE_CIRCLE_DEGREE;
        public float MaxRotateY { get; set; } = COMPLETE_CIRCLE_DEGREE;
        public float MaxRotateZ { get; set; } = COMPLETE_CIRCLE_DEGREE;
        public float MaxDragX { get; set; } = int.MaxValue;
        public float MaxDragY { get; set; } = int.MaxValue;
        public float MaxScaleX { get; set; } = int.MaxValue;
        public float MaxScaleY { get; set; } = int.MaxValue;
        public int RotateXTouches { get; set; } = 2;
        public int RotateYTouches { get; set; } = 2;
        public int RotateZTouches { get; set; } = 2;
        public int DragXTouches { get; set; } = 1;
        public int DragYTouches { get; set; } = 1;
        public bool RotateXWithDevice { get; set; } = false;
        public bool RotateYWithDevice { get; set; } = false;
        public bool RotateZWithDevice { get; set; } = false;

        float? InitialX;
        float? InitialY;

        float? InitialWidth;
        float? InitialHeight;

        float? InitialRotationX;
        float? InitialRotationY;
        float? InitialRotationZ;

        float CenterX, CenterY;

        public InteractiveCanvas()
        {
            Panning.Handle(OnPanning);
            Pinching.Handle(OnPinched);
            UserRotating.Handle(UserRotated);
        }

        async Task UserRotated(UserRotatingEventArgs args)
        {
            using (await AsyncLock.Lock())
            {
                if (CanRotateX && RotateXTouches == 2) RotateXBy(args.Degrees);

                if (CanRotateY && RotateYTouches == 2) RotateYBy(args.Degrees);

                if (CanRotateZ && RotateZTouches == 2) RotateZBy(args.Degrees);
            }
        }

        void RotateZBy(float rotatedDegree)
        {
            if (InitialRotationZ == null)
                InitialRotationZ = Rotation;

            var newRotation = (Rotation + rotatedDegree)
                .LimitWithin(InitialRotationZ.Value - MaxRotateZ, InitialRotationZ.Value + MaxRotateZ);

            this.Rotation(newRotation);
        }

        void RotateYBy(float rotatedDegree)
        {
            if (InitialRotationY == null)
                InitialRotationY = RotationY;

            var newRotationY = (RotationY + rotatedDegree)
                .LimitWithin(InitialRotationY.Value - MaxRotateY, InitialRotationY.Value + MaxRotateY);

            this.RotationY(newRotationY);
        }

        void RotateXBy(float rotatedDegree)
        {
            if (InitialRotationX == null)
                InitialRotationX = RotationX;

            var newRotationX = (RotationX + rotatedDegree)
                .LimitWithin(InitialRotationX.Value - MaxRotateX, InitialRotationX.Value + MaxRotateX);

            this.RotationX(newRotationX);
        }

        async Task OnPinched(PinchedEventArgs args)
        {
            using (await AsyncLock.Lock())
            {
                if (CanScaleX)
                    ScaleXBy(GetCenter(args.Touch1, args.Touch2), args.ChangeScale);

                if (CanScaleY)
                    ScaleYBy(GetCenter(args.Touch1, args.Touch2), args.ChangeScale);
            }
        }

        private Point GetCenter(Point touch1, Point touch2) =>
            new Point((touch1.X + touch2.X) / 2, (touch1.Y + touch2.Y) / 2);

        void ScaleYBy(Point pinchedCenter, float pinchedPixels)
        {
            if (InitialHeight == null)
                InitialHeight = ActualHeight;

            var newHeight = (ActualHeight + pinchedPixels).LimitWithin(InitialHeight.Value - MaxScaleY, InitialHeight.Value + MaxScaleY);
            var yDelta = (pinchedCenter.Y / ActualHeight) * pinchedPixels;
            var newY = ActualY - yDelta;

            // the initial position sould be fixed on scaling time.
            if (InitialY.HasValue)
                InitialY -= yDelta;

            this.Height(newHeight).Y(newY);
        }

        void ScaleXBy(Point pinchedCenter, float pinchedPixels)
        {
            if (InitialWidth == null)
                InitialWidth = ActualWidth;

            var newWidth = (ActualWidth + pinchedPixels).LimitWithin(InitialWidth.Value - MaxScaleX, InitialWidth.Value + MaxScaleX);
            var xDelta = (pinchedCenter.X / ActualWidth) * pinchedPixels;
            var newX = ActualX - xDelta;

            // the initial position sould be fixed on scaling time.
            if (InitialX.HasValue)
                InitialX -= xDelta;

            this.Width(newWidth).X(newX);
        }

        async Task OnPanning(PannedEventArgs args)
        {
            using (await AsyncLock.Lock())
            {
                if (CanDragX || CanDragY)
                {
                    if (CanDragX && args.Touches == DragXTouches)
                        DragXBy(args.To.X - args.From.X);

                    if (CanDragY && args.Touches == DragYTouches)
                        DragYBy(args.To.Y - args.From.Y);
                }
                else
                {
                    if (CanRotateX && RotateXTouches == args.Touches)
                        RotateXBy((float)((args.To.X - args.From.X) * 0.5));

                    if (CanRotateY && RotateYTouches == args.Touches)
                        RotateYBy((float)((args.To.Y - args.From.Y) * 0.5));

                    if (CanRotateZ && RotateZTouches == args.Touches)
                        RotateZBy((float)(((args.To.X - args.From.X) - (args.To.Y - args.From.Y)) * 0.5));
                }
            }
        }

        [EscapeGCop("The argument is meaningful here")]
        void DragYBy(float y)
        {
            if (!InitialY.HasValue)
                InitialY = ActualY;

            var newY = (ActualY + y).LimitWithin(InitialY.Value - GetMaxDragY(), InitialY.Value + GetMaxDragY());

            Y.Set(newY);
        }

        /// <summary>
        ///  Max drags value should be changed by scaling
        /// </summary>
        float GetMaxDragY() => MaxDragY + ActualHeight - (InitialHeight ?? ActualHeight);

        [EscapeGCop("The argument is meaningful here")]
        void DragXBy(float x)
        {

            if (!InitialX.HasValue)
                InitialX = ActualX;

            var newX = (ActualX + x).LimitWithin(InitialX.Value - GetMaxDragX(), InitialX.Value + GetMaxDragX());

            X.Set(newX);
        }

        /// <summary>
        ///  Max drags value should be changed by scaling
        /// </summary>
        float GetMaxDragX() => MaxDragX + ActualWidth - (InitialWidth ?? ActualWidth);
    }
}

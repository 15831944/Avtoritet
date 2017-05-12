namespace NewLauncher.Extension
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class PrecisionPanel : Panel
    {
        public static readonly DependencyProperty AbsHeightProperty = DependencyProperty.RegisterAttached("AbsHeight", typeof(double), typeof(PrecisionPanel), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty AbsLeftProperty = DependencyProperty.RegisterAttached("AbsLeft", typeof(double), typeof(PrecisionPanel), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty AbsTopProperty = DependencyProperty.RegisterAttached("AbsTop", typeof(double), typeof(PrecisionPanel), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty AbsWidthProperty = DependencyProperty.RegisterAttached("AbsWidth", typeof(double), typeof(PrecisionPanel), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public PrecisionPanel()
        {
            
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            for (int i = 0; i < base.InternalChildren.Count; i++)
            {
                UIElement element = base.InternalChildren[i];
                double absLeft = GetAbsLeft(element);
                double absTop = GetAbsTop(element);
                Point location = new Point(absLeft, absTop);
                double absWidth = GetAbsWidth(element);
                double absHeight = GetAbsHeight(element);
                Size size = new Size(absWidth, absHeight);
                element.Arrange(new Rect(location, size));
            }
            return base.ArrangeOverride(finalSize);
        }

        public static double GetAbsHeight(DependencyObject obj)
        {
            return (double) obj.GetValue(AbsHeightProperty);
        }

        public static double GetAbsLeft(DependencyObject obj)
        {
            return (double) obj.GetValue(AbsLeftProperty);
        }

        public static double GetAbsTop(DependencyObject obj)
        {
            return (double) obj.GetValue(AbsTopProperty);
        }

        public static double GetAbsWidth(DependencyObject obj)
        {
            return (double) obj.GetValue(AbsWidthProperty);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            double num = 0.0;
            double num2 = 0.0;
            for (int i = 0; i < base.InternalChildren.Count; i++)
            {
                UIElement element = base.InternalChildren[i];
                double absLeft = GetAbsLeft(element);
                double absTop = GetAbsTop(element);
                double absWidth = GetAbsWidth(element);
                double absHeight = GetAbsHeight(element);
                element.Measure(new Size(absWidth, absHeight));
                num = Math.Max(num, absLeft + absWidth);
                num2 = Math.Max(num2, absTop + absHeight);
            }
            return new Size(num, num2);
        }

        protected override void OnRender(DrawingContext dc)
        {
            dc.DrawRectangle(base.Background, null, new Rect(0.0, 0.0, base.RenderSize.Width, base.RenderSize.Height));
            base.OnRender(dc);
        }

        public static void SetAbsHeight(DependencyObject obj, double value)
        {
            obj.SetValue(AbsHeightProperty, value);
        }

        public static void SetAbsLeft(DependencyObject obj, double value)
        {
            obj.SetValue(AbsLeftProperty, value);
        }

        public static void SetAbsTop(DependencyObject obj, double value)
        {
            obj.SetValue(AbsTopProperty, value);
        }

        public static void SetAbsWidth(DependencyObject obj, double value)
        {
            obj.SetValue(AbsWidthProperty, value);
        }
    }
}


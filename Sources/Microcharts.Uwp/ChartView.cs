// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#if WINUI
namespace Microcharts.UI.Xaml
#else
namespace Microcharts.Uwp
#endif
{
    using SkiaSharp;
#if WINUI
    using Microsoft.UI.Xaml;
    
#if WINDOWS10_0_18362_0_OR_GREATER
    using SkiaSharp.Views.Desktop;
#else
    using SkiaSharp.Views.Windows;
#endif
#else
    using Windows.UI.Xaml;
    using SkiaSharp.Views.UWP;
#endif

    public partial class ChartView : SKXamlCanvas
    {
#region Constructors

        public ChartView()
        {
            this.PaintSurface += OnPaintCanvas;
        }

#endregion

#region Static fields

        public static readonly DependencyProperty ChartProperty = DependencyProperty.Register(nameof(Chart), typeof(Chart), typeof(ChartView), new PropertyMetadata(null, new PropertyChangedCallback(OnChartChanged)));

#endregion

#region Fields

        private InvalidatedWeakEventHandler<ChartView> handler;

        private Chart chart;

#endregion

#region Properties

        public Chart Chart
        {
            get { return (Chart)GetValue(ChartProperty); }
            set { SetValue(ChartProperty, value); }
        }

#endregion

#region Methods

        private static void OnChartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as ChartView;

            if (view.chart != null)
            {
                view.handler.Dispose();
                view.handler = null;
            }

            view.chart = e.NewValue as Chart;
            view.Invalidate();

            if (view.chart != null)
            {
                view.handler = view.chart.ObserveInvalidate(view, (v) => v.Invalidate());
            }
        }

        private void OnPaintCanvas(object sender, SKPaintSurfaceEventArgs e)
        {
            if (this.chart != null)
            {
                this.chart.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
            }
            else
            {
                e.Surface.Canvas.Clear(SKColors.Transparent);
            }
        }

#endregion
    }
}

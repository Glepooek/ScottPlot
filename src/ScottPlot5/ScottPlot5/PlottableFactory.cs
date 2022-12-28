﻿using ScottPlot.Panels;
using ScottPlot.Plottables;
using ScottPlot.Style;
using ScottPlot.DataSource;
using System.Drawing;

namespace ScottPlot;

/// <summary>
/// Helper methods to create plottable objects and add them to the plot
/// </summary>
public class PlottableFactory
{
    private readonly Plot Plot;

    public IPalette Palette { get; set; } = new Palettes.Category10();
    private int colorsUsed = 0;
    private Color NextColor => Palette.GetColor(colorsUsed++);

    public PlottableFactory(Plot plot)
    {
        Plot = plot;
    }

    public Heatmap Heatmap(double[,] intensities)
    {
        Heatmap heatmap = new(intensities);
        Plot.Plottables.Add(heatmap);
        return heatmap;
    }

    public Legend Legend(IList<LegendItem>? legendItems = null)
    {
        Legend legend = new(legendItems ?? Plot.LegendItems());
        Plot.Plottables.Add(legend);
        return legend;
    }

    public Pie Pie(IList<PieSlice> slices)
    {
        Pie pie = new(slices);
        Plot.Plottables.Add(pie);
        return pie;
    }

    public Pie Pie(IEnumerable<double> values)
    {
        var slices = values.Select(v => new PieSlice
        {
            Value = v,
            Fill = new(NextColor)
        }).ToList();
        return Pie(slices);
    }

    public Scatter Scatter(IScatterSource data, Color? color = null)
    {
        Color nextColor = color ?? NextColor;
        Scatter scatter = new(data)
        {
            MarkerColor = nextColor,
            LineColor = nextColor,
        };
        Plot.Plottables.Add(scatter);
        return scatter;
    }

    public Scatter Scatter(IReadOnlyList<double> xs, IReadOnlyList<double> ys, Color? color = null)
    {
        return Scatter(new ScatterSourceXsYs(xs, ys), color);
    }

    public Scatter Scatter(IReadOnlyList<Coordinates> coordinates, Color? color = null)
    {
        return Scatter(new ScatterSourceCoordinates(coordinates), color);
    }

    public Signal Signal(IReadOnlyList<double> ys, double period = 1, Color? color = null)
    {
        Marker marker = new(color ?? NextColor);
        return Signal(ys, period, marker);
    }

    public Signal Signal(IReadOnlyList<double> ys, double period, Marker marker)
    {
        DataSource.SignalSource data = new(ys, period);
        Signal scatter = new(data)
        {
            Marker = marker
        };
        Plot.Plottables.Add(scatter);
        return scatter;
    }

    public BarPlot Bar(IList<BarSeries> series)
    {
        var barPlot = new BarPlot(series);
        Plot.Plottables.Add(barPlot);
        return barPlot;
    }

    public BarPlot Bar(IList<Bar> bars, Fill? fill = null, string? label = null)
    {
        var serie = new BarSeries()
        {
            Bars = bars,
            Fill = fill ?? new(NextColor),
            Label = label
        };

        var series = new List<BarSeries>(1);
        series.Add(serie);

        return Bar(series);
    }

    public ColorBar ColorBar(IHasColorAxis source, Edge edge = Edge.Right)
    {
        ColorBar colorBar = new(source, edge);

        Plot.Panels.Add(colorBar);
        return colorBar;
    }
}
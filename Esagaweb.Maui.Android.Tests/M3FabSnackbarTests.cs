using Esagaweb.Maui.Android.Controls.Fab;
using Esagaweb.Maui.Android.Controls.Snackbar;
using Xunit;

namespace Esagaweb.Maui.Android.Tests;

public class M3FabSnackbarTests
{
    [Fact]
    public void ComputeOccupiedHeight_Hidden_ReturnsZero()
    {
        var margin = new Thickness(8);
        Assert.Equal(0, M3SnackbarHost.ComputeOccupiedHeight(false, 48, margin));
    }

    [Fact]
    public void ComputeOccupiedHeight_VisibleNotMeasured_ReturnsZero()
    {
        var margin = new Thickness(8);
        Assert.Equal(0, M3SnackbarHost.ComputeOccupiedHeight(true, 0, margin));
    }

    [Fact]
    public void ComputeOccupiedHeight_VisibleNotMeasuredNegative_ReturnsZero()
    {
        var margin = new Thickness(8);
        Assert.Equal(0, M3SnackbarHost.ComputeOccupiedHeight(true, -1, margin));
    }

    [Fact]
    public void ComputeOccupiedHeight_Visible48WithMargin8_Returns64()
    {
        var margin = new Thickness(8);
        Assert.Equal(64, M3SnackbarHost.ComputeOccupiedHeight(true, 48, margin));
    }

    [Fact]
    public void ComputeOccupiedHeight_Visible48WithMargin0_Returns48()
    {
        var margin = new Thickness(0);
        Assert.Equal(48, M3SnackbarHost.ComputeOccupiedHeight(true, 48, margin));
    }

    [Fact]
    public void ComputeFabTranslation_Zero_ReturnsZero()
    {
        Assert.Equal(0, M3FabHost.ComputeFabTranslation(0));
    }

    [Fact]
    public void ComputeFabTranslation_64_ReturnsNegative64()
    {
        Assert.Equal(-64, M3FabHost.ComputeFabTranslation(64));
    }

    [Fact]
    public void ComputeFabTranslation_Negative_ReturnsZero()
    {
        Assert.Equal(0, M3FabHost.ComputeFabTranslation(-10));
    }
}

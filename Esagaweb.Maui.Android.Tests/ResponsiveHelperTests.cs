using Esagaweb.Maui.Android.Helpers;
using Xunit;

namespace Esagaweb.Maui.Android.Tests;

public class ResponsiveHelperTests
{
    [Theory]
    [InlineData(0, M3Breakpoint.Compact)]
    [InlineData(320, M3Breakpoint.Compact)]
    [InlineData(599.9, M3Breakpoint.Compact)]
    [InlineData(600, M3Breakpoint.Medium)]
    [InlineData(700, M3Breakpoint.Medium)]
    [InlineData(840, M3Breakpoint.Medium)]
    [InlineData(840.1, M3Breakpoint.Expanded)]
    [InlineData(1280, M3Breakpoint.Expanded)]
    public void GetBreakpoint_RespetaLimitesM3(double widthDp, M3Breakpoint expected)
    {
        Assert.Equal(expected, ResponsiveHelper.GetBreakpoint(widthDp));
    }

    [Theory]
    [InlineData(M3Breakpoint.Compact, 16)]
    [InlineData(M3Breakpoint.Medium, 24)]
    [InlineData(M3Breakpoint.Expanded, 32)]
    public void SpacingFor_CreceConElBreakpoint(M3Breakpoint bp, double expected)
    {
        Assert.Equal(expected, ResponsiveHelper.SpacingFor(bp));
    }

    [Theory]
    [InlineData(M3Breakpoint.Compact, 1)]
    [InlineData(M3Breakpoint.Medium, 2)]
    [InlineData(M3Breakpoint.Expanded, 3)]
    public void ColumnsFor_CreceConElBreakpoint(M3Breakpoint bp, int expected)
    {
        Assert.Equal(expected, ResponsiveHelper.ColumnsFor(bp));
    }
}

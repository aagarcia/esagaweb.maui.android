using Esagaweb.Maui.Android.Controls.Common;
using Xunit;

namespace Esagaweb.Maui.Android.Tests;

public class M3ModalHostPageTests
{
    [Fact]
    public void OnBackButtonPressed_InvokesBackRequestedAndReturnsTrue()
    {
        var page = new M3ModalHostPage();
        bool invoked = false;
        page.BackRequested = () => invoked = true;

        bool result = page.SendBackButtonPressed();

        Assert.True(result);
        Assert.True(invoked);
    }

    [Fact]
    public void OnBackButtonPressed_NoBackRequested_DoesNotThrowAndReturnsTrue()
    {
        var page = new M3ModalHostPage();

        bool result = page.SendBackButtonPressed();

        Assert.True(result);
    }

    [Fact]
    public void OnBackButtonPressed_MultipleCalls_InvokesEachTime()
    {
        var page = new M3ModalHostPage();
        int count = 0;
        page.BackRequested = () => count++;

        page.SendBackButtonPressed();
        page.SendBackButtonPressed();

        Assert.Equal(2, count);
    }
}
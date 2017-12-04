var open = false;
function openSide() {
    document.getElementById("mySideNav").style.width = "260px";
    document.getElementById("mySideNav").style.left = "0px";
    open = true;
}


function closeSide() {
    if (open) {
        if ($(document).width() <= 992) {
            document.getElementById("mySideNav").style.width = "0";
            document.getElementById("mySideNav").style.left = "-30px";
            open = false;
        }
    }
}

$(window).resize(function () {
    console.log($(document).width());
    console.log($(document).height());

    if ($(document).width() > 992)
    {
        openSide();
    }
    else
    {
        closeSide();
    }
});
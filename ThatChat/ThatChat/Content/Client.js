var open = false;
function openSide() {
    document.getElementById("mySideNav").style.width = "260px";
    document.getElementById("mySideNav").style.display = "block";
    document.getElementById("mySideNav").style.height = "100%";
    document.getElementById("mySideNav").style.left = "0px";
    open = true;
}


function closeSide() {
    if ($(document).width() <= 991) {
        document.getElementById("mySideNav").style.width = "0";
        document.getElementById("mySideNav").style.left = "-30px";
        open = false;
    }
}

$(window).resize(function () {
    if ($(document).width() > 991) {
        document.getElementById("mySideNav").style.left = "-30px";
    } else if (!open) {
        document.getElementById("mySideNav").style.width = "0";
    } else {
        document.getElementById("mySideNav").style.left = "0px";
    }
});
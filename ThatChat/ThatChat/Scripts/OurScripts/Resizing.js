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
     setTimeout(function () {
        if ($(window).width() > 992) {
            openSide();
            document.getElementById("sidecontent").style.height = ($(window).height() - 110).toString() + "px";
        } else {
            closeSide();
            document.getElementById("sidecontent").style.height = "100%";
         }
        document.getElementById("discussionScrollDiv").style.height = ($(window).height() - 240).toString() + "px";
    }, 100);
});


$(document).ready(function () {
    document.getElementById("discussionScrollDiv").style.height = ($(window).height() - 240).toString() + "px";

    if ($(window).width() > 992) {
        document.getElementById("sidecontent").style.height = ($(window).height() - 110).toString() + "px";
    } else {
        closeSide();
        document.getElementById("sidecontent").style.height = "100%";
    }

});

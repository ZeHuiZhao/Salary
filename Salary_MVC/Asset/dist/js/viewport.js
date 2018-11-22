var phoneWidth = parseInt(window.screen.width);//获取屏幕的宽度
var phoneScale = phoneWidth / 750; //按750px算去比例
var ua = navigator.userAgent;   //根据各种系统机型去适配缩放，就可以保证在不容设备上看的效果都是一样的了。
if (/Android (d+.d+)/.test(ua)) {
    var version = parseFloat(RegExp.$1);
    if (version > 2.3) {
        document.write('<meta name="viewport" content="width=750, minimum-scale = ' + phoneScale + ',maximum-scale = ' + phoneScale + ', target-densitydpi=device-dpi">');
    } else {
        document.write('<meta name="viewport" content="width=750, target-densitydpi=device-dpi">');
    }
} else {
    document.write('<meta name="viewport" content="width=750, user-scalable=no, target-densitydpi=device-dpi">');
}

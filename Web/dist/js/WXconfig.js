//去除多余的分割符，例如：",a",结果为"a"
//a：字符串
//splitC：分隔符
function dropRsplit(a, splitC) {
    var tmpA = a.split(splitC);
    var tmpObj = new Array();
    for (var i = 0; i < tmpA.length; i++) {
        if (tmpA[i] != "") {
            tmpObj.push(tmpA[i]);
        }

    }
    return tmpObj.join(splitC);
}

$("#nowYear").html(new Date().getFullYear());


//获取url参数
//urlStr:url字符串
//paramName:参数名
function getUrlParam(urlStr, paramName) {
    var reg = new RegExp("(^|&|[\?])" + paramName + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = urlStr.match(reg);  //匹配目标参数
    if (r != null)
        return decodeURI(r[2]);
    return undefined; //返回参数值
}

var checkCode = "";

var prefixURL = "";
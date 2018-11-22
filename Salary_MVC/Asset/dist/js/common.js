// 公共方法

//service地址
var serviceList = [
    ''
];
var service = serviceList[0].service;

// 获取登陆信息token
window.loginInfo = JSON.parse(localStorage.getItem('dataArray'));
window.token = loginInfo && loginInfo.token;

// 取url参数
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}
function getUrl(urlStr, paramName) {
    var reg = new RegExp("(^|&|[\?])" + paramName + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = urlStr.match(reg);  //匹配目标参数
    if (r != null)
        return decodeURI(r[2]);
    return undefined; //返回参数值
}
//新开iframe
function UrlAddTabs(uid, utitle, uurl) {
    parent.$('#tab-menu .menu_tab').each(function () {
        var touid = $(this).attr('data-pageid');
        if (touid == uid) {
            $(window.parent.closeTabByPageId(uid));
        }
    })
    $(window.parent.addTabs({ id: uid, title: utitle, close: true, url: uurl, urlType: 'relative' }));
}
//关闭当前 并跳转 回去
function closeTabs(id) {
    var pageID = parent.$('.menu_tab.active').attr('data-pageid');
    var pageIDs = pageID.substring(0, 2)
    $(window.parent.refreshTabById(pageIDs));
    $(window.parent.activeTabByPageId(pageIDs));
    $(window.parent.closeTabByPageId(pageID));
}
//关闭当前 并跳转 回去 不刷新
function closeTab(id) {
    var pageID = parent.$('.menu_tab.active').attr('data-pageid');
    var pageIDs = pageID.substring(0, 2)
    var closeId = 0;
    $(".page-tabs-content").find("a.menu_tab").each(function () {
        var $a = $(this);
        if ($a.attr('data-pageid') == pageIDs) {
            closeId = 1;
            return false;//退出循环
        }
    });
    if (closeId == 1) {
        $(window.parent.activeTabByPageId(pageIDs));
    }
    $(window.parent.closeTabByPageId(pageID));
}

// 隐藏 显示 弹出窗 传id进来
function ShowUI(id) {
    $('#'+id).modal('show');
}
function HideUI(id) {
    $('#'+id).modal('hide')
}
//单选 带搜索框 select2
function selectUI(id,data) {
    $("#"+id).select2({
        data:data,
        placeholder: '请选择',
        language:'zh-CN'
    });
}

function GetOpenID(code) {
    window.codeInfo = localStorage.getItem('code');
    $.ajax({
        type: "get",
        url: service + '/api/Wechat/GetOpenID',
        async: false,
        dataType: "json",
        data: {
            'code': code
        },
        success: function (result) {
            var data = result;
            if (data.ResultType == 200) {
                code = data.AppendData;
                localStorage.setItem('openidInfo', code);
                console.log(localStorage.getItem('openidInfo'));
            }
        },
        error: function b(ms) {
            return;
        }
    });
    return code;
}

//给必填的input 增加 class= Idata
$("body").on("blur",".Idata",function(){
    //console.log(eval($(this).attr("rule")));
    var rule = new RegExp(eval($(this).attr("rule")));
    var value = $(this).val();
    var result = rule.test(value);
    //console.log(result);
    if (result) {
        $(this).removeClass("SOGWarming");
    }
    else {
        $(this).addClass("SOGWarming");
    }
    // var regPhone = /^1\d{10}$/;
   
});
$("body").on("focus",".Idata.SOGWarming",function(){
    $(this).removeClass("SOGWarming");
});
//弹出窗确认键的表单验证
function required(id){
    var success = false;
    $('#'+id+' .Idata').each(function () {
        var rule = new RegExp(eval($(this).attr("rule")));
        var value = $(this).val();
        var result = rule.test(value);
        if (result) {
            $(this).removeClass("SOGWarming");
        }
        else {
            $(this).addClass("SOGWarming");
            success = true;
        }
    });
    return success 
}

//表单排序
function dataTables(id){
    $('#'+id).dataTable({
        "searching": false,  //是否允许Datatables开启本地搜索
        "paging": false,  //是否开启本地分页
        "lengthChange": true,  //是否允许用户改变表格每页显示的记录数
        'retrieve': true,
        "info": false,   //控制是否显示表格左下角的信息
        "columnDefs": [{
            "targets": 'nosort',  //列的样式名
            "orderable": false    //包含上样式名‘nosort’的禁止排序
        }],
        //跟数组下标一样，第一列从0开始，这里表格初始化时，第四列默认降序
        "order": [1]  //asc升序   desc降序  "order": [[ 3, "desc" ]]默认第四列为降序排列
    });
}

/*** Cookie操作类 ***/
// 设置cookie
function setCookie(key, value, iDay) {
    var oDate = new Date();
    oDate.setDate(oDate.getDate() + iDay);
    document.cookie = key + "=" + value + ";expires=" + oDate;
}

// 删除cookie
function removeCookie(key) {
    this.setCookie(key, "", -1);
}

// 获取cookie
function getCookie(key) {
    var cookieArr = document.cookie.split("; ");
    for (var i = 0; i < cookieArr.length; i++) {
        var arr = cookieArr[i].split("=");
        if (arr[0] === key) {
            return arr[1];
        }
    }
    return false;
}

function SOGDate(id) {
    laydate.render({
        elem: '#' + id
        , trigger: 'click'
        , type: 'date'
        , format: 'yyyy-MM-dd'
        , theme: '#3bb9ef'
    });
}


function SOGDateTime(id) {
    laydate.render({
        elem: '#' + id
        , trigger: 'click'
        , type: 'datetime'
        , format: 'yyyy-MM-dd HH:mm'
        , theme: '#3bb9ef'
    });
}

function zlpost(url, parameter, handler,showSuccessBox) {
    showLoad();
    $.ajax({
        type: "post",
        url: url,
        data: parameter,
        dataType: "json",
        success: function (result) {
            hideLoad();
            var data = result;
            if (data.ResultType == 200) {
                if (showSuccessBox)
                    SuccessBox(data.Message);
                if (handler && typeof (handler) === "function")
                    handler(result);
            } else {
                ErrorBox(data.Message);
            }
        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            hideLoad();
            ErrorBox('当前网络较差，请刷新重试');
        }
    });
};

function zlCombCompanyAndDepartment(companyElement, deparmentElement) {
    var departmentHandler = function () {
        $.ajax({
            type: 'GET',
            url: '/Common/GetDepartmentList',
            data: {
                'companyId': $(companyElement).val()
            },
            dataType: "json",
            success: function (data) {
                if (data.ResultType == 200) {
                    var departmentList = data.AppendData;
                    $(deparmentElement).append(departmentList.map(o=> { return `<option value="${o.Id}">${o.Name}</option>` }));
                }
            }
        });
    };
    $(companyElement).on('change', function () {
        $(deparmentElement).html('<option value="">--所有部门--</option>');
        $(deparmentElement).val('');
        departmentHandler();
    });
    $.ajax({
        type: 'GET',
        url: '/Common/GetCompanyList',
        dataType: "json",
        success: function (data) {
            if (data.ResultType == 200) {
                var companyList = data.AppendData;
                $(companyElement).append(companyList.map(o=> { return `<option value="${o.Id}">${o.Name}</option>` }));
                departmentHandler();
            }
        }
    });
};

function zlCombFinanceUnit(el) {
    $.ajax({
        type: 'POST',
        url: '/FinanceUnit/GetEntityWithKeyValue',
        dataType: "json",
        success: function (data) {
            if (data.ResultType == 200) {
                var companyList = data.AppendData;
                $(el).append(companyList.map(o=> { return `<option value="${o.Id}">${o.Name}</option>` }));
            }
        }
    });
}

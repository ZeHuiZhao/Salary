

        $(".company_location img").click(function () {
            var my_company = $("#lbl_TC_COMPANY_NAME").text();
            var my_city = $("#lbl_TC_ADDRESS").text();
            // alert(my_city);
            //AddMainTab(my_company, '../BaiduMap/BaiduMap.html?city=' + my_city);
            $("#myModal_Company").html(my_company);
            $("#myModal_iframe").attr("src", '../../BaiduMap/BaiduMap.html?city=' + my_city);
            $('#myModal_location').modal();
        });
        function GetAllLecturer() {
            $.ajax({
                type: "post",
                url: "/MemberSS/ajax_APTraineeCompany1.ashx",
                data: { "action": "GetAllLecturer" },
                dataType: "json",
                async: false,
                success: function (result) {
                    //var data = $.parseJSON(result);
                    if (result.status == 1) {
                        //console.log(result);
                        var Lecturer_html="";
                        for (var i = 0; i < result.items.length;i++){
                            Lecturer_html += "<option value='" + result.items[i].UR_USER_ID + "'>" + result.items[i].UR_NAME + "</option>"
                        }
                        $("#Lecturer_select").html(Lecturer_html)
                        $("#Lecturer_select").select2();
                        $("#Lecturer_select").next(".select2").attr("id", "Lecturer_select1");
                        $("#Lecturer_select1").hide();
                        $(document).bind("click", function (e) {
                            //id为menu的是菜单，id为open的是打开菜单的按钮
                            if (!$("#Lecturer_select1").is(":hidden")) {
                                if ($(e.target).closest("#Lecturer_select1").length == 0) {
                                    //点击id为menu之外且id不是不是open，则触发
                                    var TC_Transaction_Lecturer = String($("#Lecturer_select").val());
                                    if (TC_Transaction_Lecturer == "null") {
                                        TC_Transaction_Lecturer = "";
                                    }
                                    $.ajax({
                                        type: "post",
                                        url: "/MemberSS/ajax_APTraineeCompany1.ashx",
                                        data: { "action": "SetAllLecturer", "TC_ID": getUrlParam("TC_ID"), "TC_Transaction_Lecturer": TC_Transaction_Lecturer },
                                        dataType: "json",
                                        async: false,
                                        success: function (result) {
                                            //var data = $.parseJSON(result);
                                            //console.log(result);
                                            if (result.status == 1) {
                                                SelfAlert("success", "修改成功");
                                                GetTC_Transaction_Lecturer();
                                            }
                                        }
                                    });
                                    $("#Lecturer_select").select2("close");
                                    $("#Lecturer_select1").hide();
                                    $("#lbl_TC_Transaction_Lecturer").show();
                                }
                            }
                        });
                    }                    
                }
            });
        }
        GetAllLecturer();
        $("#TC_Transaction_Lecturer1").dblclick(function () {
            //alert(123);
            $("#lbl_TC_Transaction_Lecturer").hide();
            $("#Lecturer_select1").show();
        });
        function GetTC_Transaction_Lecturer() {
            $.ajax({
                type: "post",
                url: "/MemberSS/ajax_APTraineeCompany1.ashx",
                data: { "action": "GetTC_Transaction_Lecturer", "TC_ID": getUrlParam("TC_ID") },
                dataType: "json",
                async: false,
                success: function (result) {
                    //var data = $.parseJSON(result);
                    if (result.status == 1) {
                        //console.log(result);
                        $("#lbl_TC_Transaction_Lecturer").html(result.items[0].TC_Transaction_Lecturer);
                        var my_arr = result.items[0].TC_Transaction_Lecturer1.split(",");
                        $("#Lecturer_select").val(my_arr).trigger('change');
                    }
                }
            });
        }
        GetTC_Transaction_Lecturer();

        function GetIndustry1() {
            $.ajax({
                type: "post",
                url: "/MemberSS/ajax_APTraineeCompany1.ashx",
                data: { "action": "GetIndustry", "type": "TC_Deal_Reason" },
                dataType: "json",
                async: false,
                success: function (result) {
                    var Lecturer_html = "";
                    for (var i = 0; i < result.length; i++) {
                        Lecturer_html += "<option value='" + result[i].YS_CODE + "'>" + result[i].YS_DESC + "</option>"
                    }
                    $("#view_TC_Deal_Reason_select").html(Lecturer_html)
                    $("#view_TC_Deal_Reason_select").select2();
                    $("#view_TC_Deal_Reason_select").next(".select2").attr("id", "view_TC_Deal_Reason_select1");
                    $("#view_TC_Deal_Reason_select1").hide();
                    $(document).bind("click", function (e) {
                        //id为menu的是菜单，id为open的是打开菜单的按钮
                        if (!$("#view_TC_Deal_Reason_select1").is(":hidden")) {
                            if ($(e.target).closest("#view_TC_Deal_Reason_select1").length == 0) {
                                //点击id为menu之外且id不是不是open，则触发
                                var TC_Deal_Reason = String($("#view_TC_Deal_Reason_select").val());
                                if (TC_Deal_Reason == "null") {
                                    TC_Deal_Reason = "";
                                }
                                $.ajax({
                                    type: "post",
                                    url: "/MemberSS/ajax_APTraineeCompany1.ashx",
                                    data: { "action": "SetTC_Deal_Reason", "TC_ID": getUrlParam("TC_ID"), "TC_Deal_Reason": TC_Deal_Reason },
                                    dataType: "json",
                                    async: false,
                                    success: function (result) {
                                        //var data = $.parseJSON(result);
                                        //console.log(result);
                                        if (result.status == 1) {
                                            SelfAlert("success", "修改成功");
                                            GetTC_Deal_Reason();
                                        }
                                    }
                                });
                                $("#view_TC_Deal_Reason_select").select2("close");
                                $("#view_TC_Deal_Reason_select1").hide();
                                $("#lbl_TC_Deal_Reason").show();
                            }
                        }
                    });
                }
            });
        }
        GetIndustry1();
        $("#My_view_TC_Deal_Reason").dblclick(function () {
            //alert(123);
            $("#lbl_TC_Deal_Reason").hide();
            $("#view_TC_Deal_Reason_select1").show();
        });
        function GetTC_Deal_Reason() {
            $.ajax({
                type: "post",
                url: "/MemberSS/ajax_APTraineeCompany1.ashx",
                data: { "action": "GetTC_Deal_Reason", "TC_ID": getUrlParam("TC_ID") },
                dataType: "json",
                async: false,
                success: function (result) {
                    //var data = $.parseJSON(result);
                    if (result.status == 1) {
                        //console.log(result);
                        $("#lbl_TC_Deal_Reason").html(result.items[0].TC_Deal_Reason_Desc);
                        var my_arr = result.items[0].TC_Deal_Reason.split(",");
                        $("#view_TC_Deal_Reason_select").val(my_arr).trigger('change');
                    }
                }
            });
        }
        GetTC_Deal_Reason();


        function GetCourse() {
            $.ajax({
                type: "post",
                url: "/MemberSS/ajax_APTraineeCompany1.ashx",
                data: { "action": "GetCourse", "TC_ID": getUrlParam("TC_ID") },
                dataType: "json",
                async: false,
                success: function (result) {
                    if (result.status == 1) {
                        TC_CLIENTS_TYPE = result.items[0].TC_CLIENTS_TYPE;
                        $("#lbl33_TC_CLIENTS_TYPE").html(result.items[0].TC_CLIENTS_TYPE_DESC);
                    }
                }
            });
        }
        GetCourse();

        function GetAllCourse() {
            $.ajax({
                type: "post",
                url: "/MemberSS/ajax_APTraineeCompany1.ashx",
                data: { "action": "GetAllCourse" },
                dataType: "json",
                async: false,
                success: function (result) {
                    //var data = $.parseJSON(result);
                    if (result.status == 1) {
                        //console.log(result);
                        var Lecturer_html = "<option value=''></option>";
                        for (var i = 0; i < result.items.length; i++) {
                            Lecturer_html += "<option value='" + result.items[i].CPS_ID + "'>" + result.items[i].CPS_SECTION_NAME + "</option>"
                        }
                        $("#view_TC_CLI…
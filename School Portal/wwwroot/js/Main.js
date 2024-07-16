//const { error } = require("jquery");

function login() {
    debugger
    var email = $('#email').val();
    var password = $('#password').val();

    // the dollar sign is called JQuery
    $.ajax({
        type: "POST",
        url: "/Account/Login",
        dataType: "json",
        data: {
            email: email,
            password: password
        },
        success: function (result) {
            if (result.isError) {
                debugger
                errorAlert(result.msg);
            } else {
                var url = '/Admin/Dashboard';
                successAlertWithRedirect(result.msg, url);
            }
        },
        failure: function (response) {
            alert(result.msg);
        }
    });
}

function Registration() {
    debugger
    var data = {};
    data.FirstName = $('#firstname').val();
    data.LastName = $('#lastname').val();
    data.Email = $('#email').val();
    data.Password = $('#password').val();
    data.PhoneNumber = $('#phonenumber').val();
    if (data.FirstName == "") {
        return errorAlert("First name is required");
    }
    if (data.LastName == "") {
        return errorAlert("Last name is required");
    }
    if (data.Email == "") {
        return errorAlert("Email is required");
    }
    if (data.Password == "") {
        return errorAlert("Password is required");
    }
    if (data.PhoneNumber == "") {
        return errorAlert("Phone number is required");
    }
    var userData = JSON.stringify(data);
    $.ajax({
        type: "POST",
        url: "/Account/RegisterAdmin",
        dataType: "json",
        data: {
            userDetails: userData
        },
        success: function (result) {
            if (result.isError) {
                debugger
                errorAlert(result.msg);
            } else {
                var url = '/Admin/Dashboard';
                successAlertWithRedirect(result.msg, url);
            }
        },
        failure: function (response) {
            alert(result.msg);
        }
    });
}

function GetCourseCategoryById(id) {
    $.ajax({
        type: 'GET',
        url: '/CourseCategory/EditCourseCategory',
        dataType: 'json',
        data:
        {
            id: id
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                $("#editcourseCategoryId").val(result.data.id);
                $("#editcourseCategoryName").val(result.data.name);
            }
        },
        error: function (ex) {
            "please fill the form correctly" + errorAlert(ex);
        }
    });
}



function EditcourseCategory() {
    debugger;
    var data = {};
    data.id = $('#editcourseCategoryId').val();
    data.Name = $('#editcourseCategoryName').val();
    
    if (data.Name != "" ) {
        let courseCategory = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/CourseCategory/EditedCourseCategory', // we are calling json method,
            dataType: 'json',
            data:
            {
                courseCategory: courseCategory,
            },
            success: function (result) {
                if (!result.isError) {
                    var url = '/CourseCategory/Index';
                    successAlertWithRedirect(result.msg, url);
                }
                else {
                   
                    errorAlert(result.msg);
                }
            },
            error: function (ex) {
               
                errorAlert("Network failure, please try again");
            }
            
        });
      
    }
}

function DeleteCourseCategoryById(id) {
    debugger;
    $.ajax({
        type: 'Post',
        dataType: 'json',
        url: '/CourseCategory/DeleteCourseCategoryById',
        data:
        {
            Id: id,
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var url = '/CourseCategory/Index';
                successAlertWithRedirect(result.msg, url)
            }
            else {
                errorAlert(result.msg)
            }
        },
    
    });
}


function registerTeacher() {
    debugger
    var data = {};
    data.FirstName = $('#firstName').val();
    data.LastName = $('#lastName').val();
    data.Email = $('#email').val();
    data.Address = $('#address').val();
    data.PhoneNumber = $('#phoneNumber').val();
    data.Password = $('#password').val();
    if (data.FirstName == "") {
        return errorAlert("First name is required");
    }
    if (data.LastName == "") {
        return errorAlert("Last name is required");
    }
    if (data.Email == "") {
        return errorAlert("Email is required");
    }
    if (data.Address == "") {
        return errorAlert("Address is required");
    }
    if (data.PhoneNumber == "") {
        return errorAlert("Phone number is required");
    }
    if (data.Password == "") {
        return errorAlert("Password is require");
    }

    var userData = JSON.stringify(data);
    $.ajax({
        type: "POST",
        url: "/Admin/RegisterTeacher",
        dataType: "json",
        data: {
            teacherDetails: userData
        },
        success: function (result) {
            if (result.isError) {
                debugger
                errorAlert(result.msg);
            } else {
                var url = '/Admin/Teacher';
                successAlertWithRedirect(result.msg, url);
            }
        },
        failure: function (response) {
            alert(result.msg);
        }
    });
}


//function editTeacher() {
//    debugger
//    var data = {};
//    data.id = $('#editTeacherId').val();
//    data.FirstName = $('#edit_firstName').val();
//    data.LastName = $('#edit_lastName').val();
//    data.Address = $('#edit_address').val();
//    data.PhoneNumber = $('#edit_phoneNumber').val();
//    data.Email = $('#edit_email').val();
//    if (data.FirstName != "" || data.LastName != "" || data.Address != "" || data.PhoneNumber != "" || data.Email != "") {
//        let userId = JSON.stringify(data);
//        $.ajax({
//            type: 'POST',
//            url: '/Admin/EditTeacher', // we are calling json method,
//            dataType: 'json',
//            data:
//            {
//                userId: userId,
//            },
//            success: function (result) {
//                if (!result.isError) {
//                    var url = "/Property/Index";
//                    newSuccessAlert(result.msg, url);
//                }
//                else {
                  
//                    errorAlert(result.msg);
//                }
//            },
//            error: function (ex) {
//                errorAlert("Network failure, please try again");
//            }
//        });
//    } else {
//        errorAlert("Please fill the form correctly");
//    }

//}


function GetTeacherById(id) {
    debugger
    $.ajax({
        type: 'GET',
        url: '/Admin/EditTeacher',
        dataType: 'json',
        data:
        {
            userId: id
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                $("#editTeacherId").val(result.data.id);
                $("#edit_firstName").val(result.data.firstName);
                $("#edit_lastName").val(result.data.lastName);
                $("#edit_address").val(result.data.address);
                $("#edit_phoneNumber").val(result.data.phoneNumber);
                $("#edit_email").val(result.data.email);
            }
        },
        error: function (ex) {
            "please fill the form correctly" + errorAlert(ex);
        }
    });
}

function teacherToDelete(id) {
    debugger
    $("#deleteId").val(id);
}
function deleteTeacherById() {
    debugger;
    var id = $("#deleteId").val();
    $.ajax({
        type: 'Post',
        dataType: 'json',
        url: '/Admin/deleteTeacherById',
        data:
        {
            id: id,
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var url = '/Admin/Teacher';
                successAlertWithRedirect(result.msg, url)
            }
            else {
                errorAlert(result.msg)
            }
        },

    });
}
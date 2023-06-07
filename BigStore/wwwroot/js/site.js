//token when login success
var token = 'Bearer ' + document.cookie.replace(/(?:(?:^|.*;\s*)access_token\s*\=\s*([^;]*).*$)|^.*$/, "$1");

//url local of razor page
var urlLocal = 'https://localhost:7292/';

var urlCart = urlLocal + 'Carts';

//toast message 
var messSuccess = 'Thành công!';
var messError = 'Thất bại thử lại sau!';
var messErrorService = 'Lỗi service!';

var languageTableData = {
    "decimal": "",
    "emptyTable": "Không có dữ liệu",
    "info": "Hiển thị _START_ đến _END_ của _TOTAL_ mục",
    "infoEmpty": "Hiển thị 0 đến 0 của 0 mục",
    "infoFiltered": "(được lọc từ _MAX_ mục)",
    "infoPostFix": "",
    "thousands": ",",
    "lengthMenu": "Hiển thị _MENU_ mục",
    "loadingRecords": "Đang tải...",
    "processing": "Đang xử lý...",
    "search": "Tìm kiếm:",
    "zeroRecords": "Không tìm thấy mục nào phù hợp",
    "paginate": {
        "first": "Đầu tiên",
        "last": "Cuối cùng",
        "next": "Tiếp theo",
        "previous": "Trước đó"
    },
    "aria": {
        "sortAscending": ": Sắp xếp tăng dần",
        "sortDescending": ": Sắp xếp giảm dần"
    }
}

//function call api
function ajaxRequest(url, type, data, beforeSendFunc, successFunc, errorFunc) {
    $.ajax({
        url: url,
        type: type,
        data: data,
        beforeSend: beforeSendFunc,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: successFunc,
        error: errorFunc
    });
}
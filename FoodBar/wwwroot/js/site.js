var orderId = 0;
var paymentId = 0;
function GetFood(foodId) {
    debugger;

    $("#foodId").val(foodId);
    $.ajax({
        type: 'Get',
        url: 'Food/GetFood',
        datatype: 'Json',
        data: {
            foodId: foodId
        },
        success: function (result) {
            debugger;
            $("#foodName").val(result.data.name);
            $("#amount").val(result.data.price);
            $("#accountNameId").val("Mama Ezinne Restaurant");
            $("#bankNameId").val("Zenith Bank");
            $("#accountNumberId").val("2553153651");
            $("#description").val(result.data.description);
            $("#quantity").val(1);
            $("#foodId").val(result.data.id);
        }
    });
};




function GetFoodById() {
    debugger

    var foodId = $("#foodId").val();
    var accountNumber = $("#accountNumberId").val();
    var bankName = $("#bankNameId").val();
    var accountName = $("#accountNameId").val();
    var quantity = $("#quantity").val();
    var amount = $("#amount").val();
    $.ajax({
        type: 'POST',
        url: '/Order/OrderPayment',
        dataType: 'json',
        data:
        {
            foodId: foodId, accountName: accountName, bankName: bankName, accountNumber: accountNumber, quantity: quantity, amount: amount
        },
        success: function (result) {            
            debugger;
            if (!result.isError) {
                var firstmodal = document.getElementById("modals");
                firstmodal.style.display = "none";
                $("#evidenceModal").modal('show');
                 orderId = result.data.orderId;
                 paymentId = result.data.id;
            }
            else {
                errorAlert(result.msg);
            }
        },
        error: function (ex) {

            errorAlert(ex);
        }
    })
}

function GetUploadedEvidence()
{
    debugger
    var evidence = $("#evidenceId").val();
    var realfilepath = evidence.substring(12);
    $.ajax({
        type: 'POST',
        url: '/Order/UploadEvidence',
        dataType: 'json',
        data:
        {
            orderId: orderId, paymentId: paymentId, realfilepath: realfilepath
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var url = '/Order/Index?foodId=' + result.data;
               successAlertWithRedirect(result.msg, url); 
            }
            else {
                errorAlert(result.msg);
            }
        },
        error: function (ex) {

            errorAlert(ex);
        }
    })
}



function ApproveCustomerPayment(userId,orderId) {
    debugger
    $.ajax({
        type: 'POST',
        url: '/Admin/Approve',
        dataType: 'json',
        data:
        {
            userId: userId, orderId: orderId
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var url = '/Admin/PaymentTable';
                newSuccessAlert(result.msg, url);
            }
            else {
                errorAlert(result.msg);
            }
        },
        error: function (ex) {

            errorAlert(ex);
        }
    })
}


function DeclineCustomerPayment(userId,orderId) {
    debugger
    $.ajax({
        type: 'POST',
        url: '/Admin/Decline',
        dataType: 'json',
        data:
        {
            userId: userId, orderId: orderId
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var url = '/Admin/PaymentTable';
                newSuccessAlert(result.msg, url);
            }
            else {
                errorAlert(result.msg);
            }
        },
        error: function (ex) {

            errorAlert(ex);
        }
    })
}

$("#foodList").change(function () {
    debugger
    var foodId = $('#foodList').val();
    $.ajax({
        type: 'Get',
        url: '/Admin/GetFoodDetails',
        data:
        {
            foodId: foodId, 
        },
        success: function (result) {
            $('#price').val(result.data.price);
        },
    });
});



function Quantity() {
    var quantity = $('#quantity').val();
    var foodId = $('#foodId').val();
    debugger
    $.ajax({
        type: 'GET',
        url: '/Order/GetPriceByQuantity',
        dataType: 'json',
        data:
        {
            foodId: foodId, quantity:quantity
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                $('#amount').val(result.data);
            }
            else {
                errorAlert(result.msg);
            }
        },
        error: function (ex) {

            errorAlert(ex);
        }
    })
}

function calculateTotal() {
    var quantity = parseFloat(document.getElementById("quantity").value);
    var priceFood = parseFloat(document.getElementById("priceFood").value);

    var total = isNaN(quantity) || isNaN(priceFood) ? 0 : quantity * priceFood;

    document.getElementById("total").value = isNaN(total) ? "" : total.toFixed(2);
}

function fetchFoodPrice() {
    debugger
    var foodId = $('#selectFoods').val();
    $.ajax({
        type: 'GET',
        url: '/Admin/GetPriceDetails',
        dataType: 'json',
        data:
        {
            foodId: foodId,
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                $('#priceFood').val(result.data.price);
            }
            else {
                errorAlert(result.msg);
            }
        },
        error: function (ex) {

            errorAlert(ex);
        }
    })
}

function SaveSalesRecord() {
    debugger
    var foodId = $("#selectFoods").val();
    var recordDate = $("#datetimeInput").val();
    var user = $("#selectUsers").val();
    var price = $("#priceFood").val();
    var quantity = $("#quantity").val();
    var total = $("#total").val();
   
    $.ajax({
        type: 'POST',
        url: '/Admin/SaveRecordTable',
        dataType: 'json',
        data:
        {
            foodId: foodId, recordDate: recordDate, user: user, price: price, quantity: quantity, total: total
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var url = '/Admin/KeepRecord';
                successAlertWithRedirect(result.msg, url);
            }
            else {
                errorAlert(result.msg);
            }
        },
        error: function (ex) {

            errorAlert(ex);
        }
    })
}


function viewPayment(id) {
    debugger
    if (id > 0) {
        $.ajax({
            type: 'GET',
            url: '/Admin/GetPaymentById',
            dataType: 'json',
            data:
            {
                payId: id,
            },
            success: function (result) {
                debugger;
                if (!result.isError) {
                    var res = result.data;
                    var dateTimeString = res.datePaid;
                   var date = dateTimeString.match(/^\d{4}-\d{2}-\d{2}/)[0];
                    document.getElementById("imageid").src = res.evidences;
                    $('#usernam span').text(res.user.userName);
                    $('#modal-item-Amount span').text(res.amount);
                    $('#modal-item-ReferenceNumber span').text(res.referenceNumber);
                    $('#modal-item-DatePaid span').text(date);
                    $('#itemModal').modal('show');
                }
            },
            error: function (ex) {

                errorAlert(ex);
            }
        })
    }

}
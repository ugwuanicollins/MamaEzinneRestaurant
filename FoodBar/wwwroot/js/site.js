var orderId = 0;
var paymentId = 0;


//function GetFood(foodId) {
//    debugger;

//    $("#foodId").val(foodId);
//    $.ajax({
//        type: 'Get',
//        url: 'Food/GetFood',
//        datatype: 'Json',
//        data: {
//            foodId: foodId
//        },
//        success: function (result) {
//            debugger;
//            $("#foodName").val(result.data.name);
//            $("#amount").val(result.data.price);
//            $("#accountNameId").val("Mama Ezinne Restaurant");
//            $("#bankNameId").val("Zenith Bank");
//            $("#accountNumberId").val("2553153651");
//            $("#description").val(result.data.description);
//            $("#quantity").val(1);
//            $("#foodId").val(result.data.id);
//        }
//    });
//};




function CreateOrder() {
    debugger
    var data = {};
    data.Amount = gToT;
    data.OrderDetails = JSON.stringify(orderObject);

    $.ajax({
        type: 'POST',
        url: '/Order/OrderPayment',
        dataType: 'json',
        data:
        {
            orderData: data
        },
        success: function (result) {            
            debugger;
            if (!result.isError) {
                $("#modals").modal('hide');
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
    var picture = document.getElementById("evidenceId").files[0];
    if (picture == undefined) {

        errorAlert("Upload image");
        return;
    }
    var formData = new FormData();
    formData.append("picture", picture);
    formData.append("orderId", orderId);
    formData.append("paymentId", paymentId);

    debugger
    $.ajax({
        type: 'POST',
        url: '/Order/UploadEvidence',
        dataType: 'json',
        data: formData,
        processData: false,
        contentType: false,
        success: function (result) {
            debugger;
            if (!result.isError) {
                var url = '/Order/Index?payId=' + result.payId;
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

function MapEditFood(id, name, price, desc) {
    debugger
    $('#edit_Id').val(id);
    $('#edit_name').val(name);
    $('#edit_price').val(price);
    $('#edit_description').text(desc);
    $('#edit_food').modal('show');
}

function updateFood() {
    debugger
    var data = {};
    data.id = $("#edit_Id").val();
    data.name = $("#edit_name").val();
    data.price = $("#edit_price").val();
    data.desc = $("#edit_description").val();

    var picture = document.getElementById("edit_image").files[0];
    var formData = new FormData();
    formData.append("picture", picture);
    formData.append("id", data.id);
    formData.append("name", data.name);
    formData.append("price", data.price);
    formData.append("description", data.desc);
    if (data.id > 0 && data.name != "" && data.price != "" && data.desc != "") {
        $.ajax({
            type: 'POST',
            url: '/Admin/Edit',
            dataType: 'json',
            data: formData,
            processData: false,
            contentType: false,
            success: function (result) {
                debugger;
                if (!result.isError) {
                    var url = '/Admin/ManageFood';
                    successAlertWithRedirect(result.msg, url);
                    window.location.reload;
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
    errorAlert("Fill in the form correctly");
}     

function DeleteFood(id) {
    debugger
    $.ajax({
        type: 'POST',
        url: '/Admin/DeleteFood',
        dataType: 'json',
        data:
        {
            id: id,
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var url = "/Admin/ManageFood";
                successAlert(result.msg, url);
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
let cartItems = [];
let cartCount = 0;

function addToCart(foodId) {
    debugger;
    if (!cartItems.includes(foodId))
    {
        cartItems.push(foodId);
        cartCount = cartItems.length;
        $('#cart-item-count span').text('');
        $('#cart-item-count span').text(cartCount);
    }
    var result = "";
    if (cartCount > 0) {
        $.each(cartItems, function (index, x) {
            result += x + ",";
        });

        result = result.slice(0, -1);
        result = "https://localhost:7113/Cart/AddToCart?foodIds=" + result;
    }
    // Select the target element using a jQuery selector
    var $linkElement = $('#linkToClick');

    // Replace the href attribute value
    $linkElement.attr('href', result);
}





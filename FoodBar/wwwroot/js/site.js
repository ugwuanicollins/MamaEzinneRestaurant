var orderId = 0;
var paymentId = 0;


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
    const button = document.getElementById('uploadBtn');
    var defaultBtn = button.innerHTML;
    button.disabled = true;
    var spinner = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>';
    button.innerHTML = spinner;
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
                button.disabled = false;
                button.innerHTML = defaultBtn;
                localStorage.removeItem("foodKey");
                var url = '/Order/Index?payId=' + result.payId;
               successAlertWithRedirect(result.msg, url); 
            }
            else {
                button.disabled = false;
                button.innerHTML = defaultBtn;
                errorAlert(result.msg);
            }
        },
        error: function (ex) {
            button.disabled = false;
            button.innerHTML = defaultBtn;
            errorAlert(ex);
        }
    })
}

function ShowEvidenceModal(order_Id, payment_Id) {
    orderId = order_Id;
    paymentId = payment_Id;
    $("#evidenceModal").modal('show');
}

function ApproveCustomerPayment(orderId) {
    debugger
    const button = document.getElementById('approvedBtn');
    var defaultBtn = button.innerHTML;
    button.disabled = true;
    var spinner = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>Loading...';
    button.innerHTML = spinner;
    $.ajax({
        type: 'POST',
        url: '/Admin/Approve',
        dataType: 'json',
        data:
        {
            orderId: orderId
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                button.disabled = false;
                button.innerHTML = defaultBtn;
                var url = '/Admin/PaymentTable';
                newSuccessAlert(result.msg, url);
            }
            else {
                button.disabled = false;
                button.innerHTML = defaultBtn;
                errorAlert(result.msg);
            }
        },
        error: function (ex) {
            button.disabled = false;
            button.innerHTML = defaultBtn;
            errorAlert(ex);
        }
    })
}


function DeclineCustomerPayment(orderId) {
    debugger
    const button = document.getElementById('declinedBtn');
    var defaultBtn = button.innerHTML;
    button.disabled = true;
    var spinner = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>Loading...';
    button.innerHTML = spinner;
    $.ajax({
        type: 'POST',
        url: '/Admin/Decline',
        dataType: 'json',
        data:
        {
           orderId: orderId
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                button.disabled = false;
                button.innerHTML = defaultBtn;
                var url = '/Admin/PaymentTable';
                newSuccessAlert(result.msg, url);
            }
            else {
                button.disabled = false;
                button.innerHTML = defaultBtn;
                errorAlert(result.msg);
            }
        },
        error: function (ex) {
            button.disabled = false;
            button.innerHTML = defaultBtn;
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

    var picture = document.getElementById("edit_image").files[0]; //
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
let cartItems = [];
let cartCount = 0;

function addToCart(foodId) {
    debugger;
    if (foodId != "") {
        if (!cartItems.includes(foodId)) {
            cartItems.push(foodId);
            localStorage.setItem("foodKey", JSON.stringify(cartItems));
            cartCount = cartItems.length;
            $('#cart-item-count span').text('');
            $('#cart-item-count span').text(cartCount);
        }
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


function continueShopping() {
    debugger
    var foodObject = localStorage.getItem("foodKey");
    if (foodObject != null) {
        cartItems = JSON.parse(foodObject);
        cartCount = cartItems.length;
        $('#cart-item-count span').text(cartCount);
        addToCart("");
    }
}

function updateCartItem(foodId) {
    debugger
    var foodObject = localStorage.getItem("foodKey");
    cartItems = JSON.parse(foodObject);
    var index = cartItems.indexOf(foodId);
    cartItems.splice(index, 1);
    cartCount = cartItems.length;
    $('#cart-item-count span').text(cartCount);
    localStorage.setItem("foodKey", JSON.stringify(cartItems));
    addToCart("");
}

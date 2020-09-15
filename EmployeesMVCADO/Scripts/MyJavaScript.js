function SortData(sort) {
    document.getElementById('myvalue').value = sort;
}

function MyFunction(dataID) {
    swal({
        title: "Are You sure ?",
        text: "Your Data is Goig To Be Deleted",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: "Home/Delete/" + dataID,
                    type: 'post',
                    success: function (result) {
                        console.log(result);
                        if (result) {
                            swal("Your Data deleted successfully! ", {
                                icon: "success",
                            });
                            let row = $("[data-rowid=" + dataID + "]");
                            row.remove();
                        }
                        else {
                            swal("The Data Is Not Deleted");
                        }
                    }
                });

            } else {
                swal("The Data Is Not Deleted");
            }
        });
}


$('#submit').on("click", function () {    
    var first_name = $('#EmpName').val();
    var age = $('#EmpAge').val();
    var email = $('#EmpEmail').val();
    var salary = $('#EmpSalary').val();
    var reEmail = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
    var reName = /^[A-Za-z]+$/;
    var breakI = 0;

    $(".error").remove();

    if (first_name.length == "") {
        $('#EmpName').after('<span class="error">This field is required</span>');
        breakI++;
    }
    else if (!first_name.match(reName)) {
        $('#EmpName').after('<span class="error"> Enter Valid Name</span>');
        breakI++;
    }

    if (age.length < 1) {
        $('#EmpAge').after('<span class="error">This field is required</span>');      
        breakI++;
    }

    if (email.length < 1) {
        $('#EmpEmail').after('<span class="error">This field is required</span>');
        breakI++;
    }
    else if (!email.match(reEmail)) {
        $('#EmpEmail').after('<span class="error">Enter Valid Email</span>');
        breakI++;
    }
  

    if (salary.length < 1) {
        $('#EmpSalary').after('<span class="error">This field is required</span>');
        breakI++;
    }

    if (!breakI) {
        $(this).removeAttr("type").attr("type", "submit");       
    }

});
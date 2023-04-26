$(document).ready(function () {
    $(document).on("click", '.open-book-model', function (e) {

        e.preventDefault();
        var url = $(this).attr("href");

        fetch(url)
            .then(response => response.text())
            .then(modalHtml => {
                $("#quickModal .modal-dialog").html(modalHtml)
            });


        $("#quickModal").modal("show")
    })
    $(document).on("click", '.add-basket', function (e) {

        e.preventDefault();
        var url = $(this).attr("href");
        let basketNumber = $(".text-number")[0];
        fetch(url)
            .then(response => response.text())
            .then(html => $("#basket-cart").html(html))

    })

    $(document).on("click", ".remove-basket", function (e) {
        let url = "/book/removebasket/" + $(this).attr("data-id")

        fetch(url)
            .then(response => {
                if (!response.ok) {
                    alert("Xeta Bas Verdi")
                    return
                }
                else {
                    return response.json()
                }
            }).then(data => {
                let parents = $(this).parents(".single-cart-block");
                parents[0].remove()
                $(".cart-total .price").text(data.totalPrice)
                $(".cart-total .text-number").text(data.count)

            })
    })
})
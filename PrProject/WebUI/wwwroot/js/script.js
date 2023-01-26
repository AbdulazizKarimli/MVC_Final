const loadMore = document.getElementById("loadMore");
const productList = document.getElementById("productList");
const countInput = document.getElementById("countInput");

let count = 8;
loadMore.addEventListener("click", function () {
    fetch(`/Product/LoadMore?skip=${count}`).then(data => data.text())
        .then(response => {
            productList.innerHTML += response;
            count += 8;

            console.log(count);

            if (count >= countInput.value) {
                loadMore.remove();
            }
        })
})
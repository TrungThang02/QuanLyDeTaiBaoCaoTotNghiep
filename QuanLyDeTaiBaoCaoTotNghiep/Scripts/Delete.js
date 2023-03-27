const btnDelete = document.querySelectorAll('.delete');

btnDelete.forEach(btn => {
    btn.addEventListener('click', (event) => {
        var id = btn.dataset.id;
        event.preventDefault();

        Swal.fire({
            title: 'Bạn có chắc chắn muốn xóa ?',
            text: "Nếu bạn xóa bạn không thể khôi phục lại!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes !'
        }).then((result) => {
            if (result.isConfirmed) {
                Swal.fire(
                    'Xóa thành công'
                )
                window.location.href = "/Documents/Delete/" + id;
            }
        })
    });
});
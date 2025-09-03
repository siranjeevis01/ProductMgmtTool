// Cart functionality
$(document).ready(function() {
    updateCartCount();
    
    $('.plus-btn').click(function() {
        var input = $(this).siblings('.quantity-input');
        var value = parseInt(input.val());
        input.val(value + 1);
    });
    
    $('.minus-btn').click(function() {
        var input = $(this).siblings('.quantity-input');
        var value = parseInt(input.val());
        if (value > 1) {
            input.val(value - 1);
        }
    });
    
    // Add to cart
    $('.add-cart-btn').click(function() {
        var productId = $(this).data('product-id');
        var quantity = $(this).closest('.add-to-cart').find('.quantity-input').val();
        
        $.post('@Url.Action("AddItem", "Cart")', {
            productId: productId,
            quantity: parseInt(quantity)
        }, function(response) {
            if (response.success) {
                showToast('Product added to cart!', 'success');
                
                $('#cartCount').text(response.itemCount);
            }
        }).fail(function() {
            showToast('Error adding product to cart', 'error');
        });
    });
    
    function updateCartCount() {
        $.get('@Url.Action("GetCartItemCount", "Cart")', function(count) {
            $('#cartCount').text(count);
        });
    }
    
    function showToast(message, type) {
        // Simple toast implementation
        var toast = $('<div class="toast-alert alert alert-' + (type === 'success' ? 'success' : 'danger') + '">' + message + '</div>');
        toast.css({
            position: 'fixed',
            top: '20px',
            right: '20px',
            zIndex: 9999
        });
        
        $('body').append(toast);
        
        setTimeout(function() {
            toast.fadeOut(function() {
                $(this).remove();
            });
        }, 3000);
    }
});
package com.example.management.domain;

import java.math.BigDecimal;
import java.util.Objects;

public final class OrderLine {

    private final String productId;
    private final int quantity;
    private final BigDecimal unitPrice;

    private OrderLine(String productId, int quantity, BigDecimal unitPrice) {
        this.productId = productId;
        this.quantity = quantity;
        this.unitPrice = unitPrice;
    }

    public static OrderLine of(String productId, int quantity, BigDecimal unitPrice) {
        if (productId == null || productId.isBlank()) {
            throw new IllegalArgumentException("productId must not be null or blank");
        }
        if (quantity <= 0) {
            throw new IllegalArgumentException("quantity must be greater than zero");
        }
        Objects.requireNonNull(unitPrice, "unitPrice must not be null");
        if (unitPrice.compareTo(BigDecimal.ZERO) < 0) {
            throw new IllegalArgumentException("unitPrice must not be negative");
        }
        return new OrderLine(productId, quantity, unitPrice);
    }

    public String getProductId() {
        return productId;
    }

    public int getQuantity() {
        return quantity;
    }

    public BigDecimal getUnitPrice() {
        return unitPrice;
    }

    public BigDecimal subtotal() {
        return unitPrice.multiply(BigDecimal.valueOf(quantity));
    }
}


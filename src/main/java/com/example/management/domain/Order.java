package com.example.management.domain;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public final class Order {

    private final String id;
    private final String customerId;
    private final List<OrderLine> lines;
    private OrderStatus status;

    private Order(String id, String customerId, List<OrderLine> lines, OrderStatus status) {
        this.id = id;
        this.customerId = customerId;
        this.lines = lines;
        this.status = status;
    }

    public static Order create(String id, String customerId, List<OrderLine> lines) {
        if (id == null || id.isBlank()) {
            throw new IllegalArgumentException("id must not be null or blank");
        }
        if (customerId == null || customerId.isBlank()) {
            throw new IllegalArgumentException("customerId must not be null or blank");
        }
        if (lines == null) {
            throw new IllegalArgumentException("lines must not be null");
        }
        if (lines.isEmpty()) {
            throw new IllegalArgumentException("Order must contain at least one line");
        }

        List<OrderLine> defensiveCopy = Collections.unmodifiableList(new ArrayList<>(lines));
        return new Order(id, customerId, defensiveCopy, OrderStatus.CREATED);
    }

    public String getId() {
        return id;
    }

    public String getCustomerId() {
        return customerId;
    }

    public List<OrderLine> getLines() {
        return lines;
    }

    public OrderStatus getStatus() {
        return status;
    }

    public BigDecimal totalAmount() {
        return lines.stream()
                .map(OrderLine::subtotal)
                .reduce(BigDecimal.ZERO, BigDecimal::add);
    }

    public void pay() {
        ensureNotCancelled();
        if (status != OrderStatus.CREATED) {
            throw new IllegalStateException("Order must be in CREATED status to pay");
        }
        status = OrderStatus.PAID;
    }

    public void ship() {
        ensureNotCancelled();
        if (status != OrderStatus.PAID) {
            throw new IllegalStateException("Order must be in PAID status to ship");
        }
        status = OrderStatus.SHIPPED;
    }

    public void cancel() {
        if (status == OrderStatus.CANCELLED) {
            return;
        }
        if (status == OrderStatus.SHIPPED) {
            throw new IllegalStateException("Cannot cancel an order that is already shipped");
        }
        status = OrderStatus.CANCELLED;
    }

    private void ensureNotCancelled() {
        if (status == OrderStatus.CANCELLED) {
            throw new IllegalStateException("Cannot change status of a cancelled order");
        }
    }
}


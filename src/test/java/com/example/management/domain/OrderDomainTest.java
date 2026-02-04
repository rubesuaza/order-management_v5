package com.example.management.domain;

import org.junit.jupiter.api.Nested;
import org.junit.jupiter.api.Test;

import java.math.BigDecimal;
import java.util.List;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;

class OrderDomainTest {

    @Nested
    class OrderLineInvariants {

        @Test
        void cannotCreateOrderLineWithNonPositiveQuantity() {
            assertThrows(IllegalArgumentException.class,
                    () -> OrderLine.of("product-1", 0, new BigDecimal("10.00")));

            assertThrows(IllegalArgumentException.class,
                    () -> OrderLine.of("product-1", -1, new BigDecimal("10.00")));
        }

        @Test
        void cannotCreateOrderLineWithNegativePrice() {
            assertThrows(IllegalArgumentException.class,
                    () -> OrderLine.of("product-1", 1, new BigDecimal("-1.00")));
        }

        @Test
        void subtotalIsQuantityTimesUnitPrice() {
            OrderLine line = OrderLine.of("product-1", 3, new BigDecimal("15.50"));

            assertEquals(new BigDecimal("46.50"), line.subtotal());
        }
    }

    @Nested
    class OrderInvariants {

        @Test
        void cannotCreateOrderWithoutLines() {
            assertThrows(IllegalArgumentException.class,
                    () -> Order.create("order-1", "customer-1", List.of()));
        }

        @Test
        void cannotCreateOrderWithNullArguments() {
            OrderLine line = OrderLine.of("product-1", 1, new BigDecimal("10.00"));

            assertThrows(IllegalArgumentException.class,
                    () -> Order.create(null, "customer-1", List.of(line)));

            assertThrows(IllegalArgumentException.class,
                    () -> Order.create("order-1", null, List.of(line)));

            assertThrows(IllegalArgumentException.class,
                    () -> Order.create("order-1", "customer-1", null));
        }

        @Test
        void orderTotalIsSumOfLineSubtotals() {
            OrderLine line1 = OrderLine.of("product-1", 2, new BigDecimal("10.00"));
            OrderLine line2 = OrderLine.of("product-2", 1, new BigDecimal("5.50"));

            Order order = Order.create("order-1", "customer-1", List.of(line1, line2));

            assertEquals(new BigDecimal("25.50"), order.totalAmount());
        }
    }

    @Nested
    class OrderStatusTransitions {

        @Test
        void defaultStatusIsCreated() {
            OrderLine line = OrderLine.of("product-1", 1, new BigDecimal("10.00"));

            Order order = Order.create("order-1", "customer-1", List.of(line));

            assertEquals(OrderStatus.CREATED, order.getStatus());
        }

        @Test
        void createdToPaidToShippedIsAllowed() {
            OrderLine line = OrderLine.of("product-1", 1, new BigDecimal("10.00"));
            Order order = Order.create("order-1", "customer-1", List.of(line));

            order.pay();
            assertEquals(OrderStatus.PAID, order.getStatus());

            order.ship();
            assertEquals(OrderStatus.SHIPPED, order.getStatus());
        }

        @Test
        void cannotShipBeforePaying() {
            OrderLine line = OrderLine.of("product-1", 1, new BigDecimal("10.00"));
            Order order = Order.create("order-1", "customer-1", List.of(line));

            assertThrows(IllegalStateException.class, order::ship);
        }

        @Test
        void cannotChangeStatusAfterCancellation() {
            OrderLine line = OrderLine.of("product-1", 1, new BigDecimal("10.00"));
            Order order = Order.create("order-1", "customer-1", List.of(line));

            order.cancel();
            assertEquals(OrderStatus.CANCELLED, order.getStatus());

            assertThrows(IllegalStateException.class, order::pay);
            assertThrows(IllegalStateException.class, order::ship);
        }
    }
}


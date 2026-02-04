package com.example.management.infrastructure.persistence;

import com.example.management.domain.Order;
import com.example.management.domain.OrderLine;
import com.example.management.domain.OrderRepository;
import com.example.management.domain.OrderStatus;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.annotation.DirtiesContext;

import java.math.BigDecimal;
import java.util.List;
import java.util.Optional;

import static org.assertj.core.api.Assertions.assertThat;

@SpringBootTest
@DirtiesContext(classMode = DirtiesContext.ClassMode.AFTER_EACH_TEST_METHOD)
class JpaOrderRepositoryTest {

    @Autowired
    private OrderRepository orderRepository;

    @Test
    void savesAndLoadsOrderWithLines() {
        OrderLine line1 = OrderLine.of("product-1", 2, new BigDecimal("10.00"));
        OrderLine line2 = OrderLine.of("product-2", 1, new BigDecimal("5.50"));

        Order order = Order.create("order-1", "customer-1", List.of(line1, line2));

        orderRepository.save(order);

        Optional<Order> reloaded = orderRepository.findById("order-1");

        assertThat(reloaded).isPresent();
        Order found = reloaded.orElseThrow();
        assertThat(found.getId()).isEqualTo("order-1");
        assertThat(found.getCustomerId()).isEqualTo("customer-1");
        assertThat(found.getStatus()).isEqualTo(OrderStatus.CREATED);
        assertThat(found.getLines()).hasSize(2);
        assertThat(found.totalAmount()).isEqualByComparingTo(new BigDecimal("25.50"));
    }

    @Test
    void findsOrdersByCustomerId() {
        OrderLine line = OrderLine.of("product-1", 1, new BigDecimal("10.00"));

        Order order1 = Order.create("order-1", "customer-1", List.of(line));
        Order order2 = Order.create("order-2", "customer-1", List.of(line));
        Order order3 = Order.create("order-3", "customer-2", List.of(line));

        orderRepository.save(order1);
        orderRepository.save(order2);
        orderRepository.save(order3);

        List<Order> customer1Orders = orderRepository.findByCustomerId("customer-1");

        assertThat(customer1Orders)
                .hasSize(2)
                .extracting(Order::getId)
                .containsExactlyInAnyOrder("order-1", "order-2");
    }
}


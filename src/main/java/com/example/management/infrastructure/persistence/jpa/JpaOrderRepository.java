package com.example.management.infrastructure.persistence.jpa;

import com.example.management.domain.Order;
import com.example.management.domain.OrderLine;
import com.example.management.domain.OrderRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

@Repository
public class JpaOrderRepository implements OrderRepository {

    private final SpringDataOrderJpaRepository jpaRepository;

    public JpaOrderRepository(SpringDataOrderJpaRepository jpaRepository) {
        this.jpaRepository = jpaRepository;
    }

    @Override
    public Order save(Order order) {
        OrderEntity entity = toEntity(order);
        OrderEntity saved = jpaRepository.save(entity);
        return toDomain(saved);
    }

    @Override
    public Optional<Order> findById(String id) {
        return jpaRepository.findById(id).map(this::toDomain);
    }

    @Override
    public List<Order> findByCustomerId(String customerId) {
        return jpaRepository.findByCustomerId(customerId)
                .stream()
                .map(this::toDomain)
                .toList();
    }

    private OrderEntity toEntity(Order order) {
        List<OrderLineEmbeddable> lineEmbeddables = order.getLines().stream()
                .map(this::toEmbeddable)
                .toList();

        return new OrderEntity(order.getId(), order.getCustomerId(), order.getStatus(), lineEmbeddables);
    }

    private OrderLineEmbeddable toEmbeddable(OrderLine line) {
        return new OrderLineEmbeddable(line.getProductId(), line.getQuantity(), line.getUnitPrice());
    }

    private Order toDomain(OrderEntity entity) {
        List<OrderLine> lines = entity.getLines().stream()
                .map(this::toDomainLine)
                .toList();

        // Creamos la orden usando la fábrica de dominio y luego sincronizamos el estado
        Order order = Order.create(entity.getId(), entity.getCustomerId(), lines);

        // El único estado mutable en Order es status; lo actualizamos si es diferente al inicial
        switch (entity.getStatus()) {
            case CREATED -> {
                // ya es el estado por defecto
            }
            case PAID -> order.pay();
            case SHIPPED -> {
                order.pay();
                order.ship();
            }
            case CANCELLED -> order.cancel();
        }

        return order;
    }

    private OrderLine toDomainLine(OrderLineEmbeddable embeddable) {
        return OrderLine.of(embeddable.getProductId(), embeddable.getQuantity(), embeddable.getUnitPrice());
    }
}


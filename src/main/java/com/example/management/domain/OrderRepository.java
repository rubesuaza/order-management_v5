package com.example.management.domain;

import java.util.List;
import java.util.Optional;

/**
 * Puerto de repositorio para la agregación Order.
 * <p>
 * La infraestructura (por ejemplo, JPA, JDBC, etc.) debe proporcionar
 * una implementación concreta de esta interfaz.
 */
public interface OrderRepository {

    Order save(Order order);

    Optional<Order> findById(String id);

    List<Order> findByCustomerId(String customerId);
}


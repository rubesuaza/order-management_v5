package com.example.management.infrastructure.persistence.jpa;

import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;

public interface SpringDataOrderJpaRepository extends JpaRepository<OrderEntity, String> {

    List<OrderEntity> findByCustomerId(String customerId);
}


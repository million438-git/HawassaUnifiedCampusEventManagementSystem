-- ============================================================================
-- HUCEMS Schema Migration: user_relationships Table
-- Supports Multi-User Follows, Connections, and Community Graph
-- ============================================================================

CREATE TABLE IF NOT EXISTS `user_relationships` (
    `id` BIGINT UNSIGNED NOT NULL AUTO_INCREMENT,
    `follower_user_id` BIGINT UNSIGNED NOT NULL,
    `followed_user_id` BIGINT UNSIGNED NOT NULL,
    `created_at` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`id`),
    UNIQUE KEY `uq_user_relationships_pair` (`follower_user_id`, `followed_user_id`),
    KEY `idx_user_relationships_follower` (`follower_user_id`),
    KEY `idx_user_relationships_followed` (`followed_user_id`),
    CONSTRAINT `fk_user_rel_follower` FOREIGN KEY (`follower_user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT `fk_user_rel_followed` FOREIGN KEY (`followed_user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

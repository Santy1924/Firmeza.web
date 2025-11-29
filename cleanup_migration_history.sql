-- Additional cleanup: Remove migration history
DROP TABLE IF EXISTS public."__EFMigrationsHistory" CASCADE;

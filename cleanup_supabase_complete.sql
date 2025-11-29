-- Complete cleanup script for Supabase
-- This will remove ALL tables to start fresh

DROP TABLE IF EXISTS public."DetallesVenta" CASCADE;
DROP TABLE IF EXISTS public."Ventas" CASCADE;
DROP TABLE IF EXISTS public."Clientes" CASCADE;
DROP TABLE IF EXISTS public."Productos" CASCADE;
DROP TABLE IF EXISTS public."AspNetUserTokens" CASCADE;
DROP TABLE IF EXISTS public."AspNetUserRoles" CASCADE;
DROP TABLE IF EXISTS public."AspNetUserLogins" CASCADE;
DROP TABLE IF EXISTS public."AspNetUserClaims" CASCADE;
DROP TABLE IF EXISTS public."AspNetRoleClaims" CASCADE;
DROP TABLE IF EXISTS public."AspNetUsers" CASCADE;
DROP TABLE IF EXISTS public."AspNetRoles" CASCADE;
DROP TABLE IF EXISTS public."__EFMigrationsHistory" CASCADE;

-- Also drop if they exist without quotes (case-insensitive)
DROP TABLE IF EXISTS public.DetallesVenta CASCADE;
DROP TABLE IF EXISTS public.Ventas CASCADE;
DROP TABLE IF EXISTS public.Clientes CASCADE;
DROP TABLE IF EXISTS public.Productos CASCADE;
DROP TABLE IF EXISTS public.AspNetUserTokens CASCADE;
DROP TABLE IF EXISTS public.AspNetUserRoles CASCADE;
DROP TABLE IF EXISTS public.AspNetUserLogins CASCADE;
DROP TABLE IF EXISTS public.AspNetUserClaims CASCADE;
DROP TABLE IF EXISTS public.AspNetRoleClaims CASCADE;
DROP TABLE IF EXISTS public.AspNetUsers CASCADE;
DROP TABLE IF EXISTS public.AspNetRoles CASCADE;
DROP TABLE IF EXISTS public.__EFMigrationsHistory CASCADE;

// Copyright © Spatial Corporation. All rights reserved.

import { SESSION_COOKIE_NAME } from "@sptlco/client";
import { routing } from "./locales/routing";
import { jwtVerify } from "jose";
import createMiddleware from "next-intl/middleware";
import { NextRequest, NextResponse } from "next/server";

const intl = createMiddleware(routing);

type User = "member" | "guest";
type Target = User | User[];
type Rule = { pattern: string; allow?: Target; block?: Target; redirect: string };

const match = (pattern: string, path: string): boolean => {
  const segments = path.split("/").filter(Boolean);

  if (segments.length > 0 && routing.locales.includes(segments[0] as any)) {
    path = "/" + segments.slice(1).join("/");
  }

  const escaped = pattern.replace(/[-/\\^$+?.()|[\]{}]/g, "\\$&");
  const regex = `^${escaped.replace(/\*/g, ".*")}$`;

  return new RegExp(regex).test(path);
};

const normalize = (target?: Target): User[] => {
  if (!target) {
    return [];
  }

  return Array.isArray(target) ? target : [target];
};

const extract = async (request: NextRequest): Promise<User> => {
  const cookie = request.cookies.get(SESSION_COOKIE_NAME)?.value;

  if (!cookie) {
    return "guest";
  }

  try {
    await jwtVerify(cookie, new TextEncoder().encode(process.env.JWT_SECRET));
    return "member";
  } catch {
    return "guest";
  }
};

export default async (request: NextRequest) => {
  const response = intl(request);
  const path = request.nextUrl.pathname;

  const rule = rules.find((rule) => match(rule.pattern, path));

  if (!rule) {
    return response;
  }

  const role = await extract(request);
  const allow = normalize(rule.allow);
  const block = normalize(rule.block);

  if (block.includes(role) || (allow.length > 0 && !allow.includes(role))) {
    const url = new URL(rule.redirect, request.url);
    const origin = request.nextUrl.searchParams.get("origin");

    if (origin) {
      return NextResponse.redirect(decodeURIComponent(origin));
    }

    url.searchParams.set("origin", encodeURIComponent(request.url));

    return NextResponse.redirect(url);
  }

  return response;
};

export const config = {
  matcher: "/((?!api|trpc|_next|_vercel|.*\\..*).*)"
};

const rules: Rule[] = [
  {
    pattern: "/platform/management*",
    allow: "member",
    redirect: "/session/new"
  },
  {
    pattern: "/session/new",
    allow: "guest",
    redirect: "/"
  }
];

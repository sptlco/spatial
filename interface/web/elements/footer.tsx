// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { clsx } from "clsx";
import { useState } from "react";

import { Button, Container, createElement, Field, Form, Icon, Link, Logo, Path, Span, Svg } from "@sptlco/design";

type Hyperlink = {
  category: string;
  href: string;
  label: string;
};

/**
 * A shared page footer.
 */
export const Footer = createElement<"footer">((props, ref) => {
  const [email, setEmail] = useState("");

  const links: Hyperlink[] = [
    { category: "Product", label: "Overview", href: "/overview" },
    { category: "Product", label: "Pricing", href: "/pricing" },
    { category: "Product", label: "Features", href: "/features" },
    { category: "Product", label: "Changelog", href: "/changelog" },
    { category: "Product", label: "Roadmap", href: "/roadmap" },

    { category: "Company", label: "About", href: "/about" },
    { category: "Company", label: "Careers", href: "/careers" },
    { category: "Company", label: "Press", href: "/press" },
    { category: "Company", label: "Partners", href: "/partners" },
    { category: "Company", label: "Contact", href: "/contact" },

    { category: "Resources", label: "Blog", href: "/blog" },
    { category: "Resources", label: "Documentation", href: "/docs" },
    { category: "Resources", label: "Guides", href: "/guides" },
    { category: "Resources", label: "Help Center", href: "/help" },
    { category: "Resources", label: "Community", href: "/community" },
    { category: "Resources", label: "Status", href: "/status" },

    { category: "Developers", label: "API Reference", href: "/developers/api" },
    { category: "Developers", label: "SDK", href: "/developers/sdks" },
    { category: "Developers", label: "CLI", href: "/developers/cli" },
    { category: "Developers", label: "GitHub", href: "https://github.com/spatial" },
    { category: "Developers", label: "Webhooks", href: "/developers/webhooks" },

    { category: "Compliance", label: "Privacy Policy", href: "/legal/privacy" },
    { category: "Compliance", label: "User Agreement", href: "/legal/terms" },
    { category: "Compliance", label: "Cookie Policy", href: "/legal/cookies" }
  ];

  const groups = links.reduce<Record<string, Hyperlink[]>>((acc, link) => {
    if (!acc[link.category]) {
      acc[link.category] = [];
    }

    acc[link.category].push(link);

    return acc;
  }, {});

  return (
    <footer {...props} ref={ref} className="flex flex-col items-center border-t border-line-base">
      <Container className="flex flex-col sm:flex-row relative gap-10 p-10 w-full xl:max-w-7xl xl:border-l xl:border-r border-line-base">
        <Span className="absolute top-0 left-0 size-1.5 -translate-x-1/2 -translate-y-1/2 flex xl:bg-foreground-secondary" />
        <Span className="absolute top-0 right-0 size-1.5 translate-x-1/2 -translate-y-1/2 flex xl:bg-foreground-secondary" />
        <Container className="flex flex-col gap-10 grow items-start justify-between">
          <Container className="flex flex-col gap-10">
            <Logo mode="wordmark" className="h-10 md:h-16" />
            <Container className="flex flex-col text-sm font-semibold uppercase">
              <Span>240 2nd Avenue S</Span>
              <Span>STE 201K</Span>
              <Span>Seattle, WA 98104</Span>
            </Container>
          </Container>
          <Container className="flex flex-col gap-4">
            <Span className="text-foreground-tertiary text-sm">© Spatial Corporation. All rights reserved.</Span>
            <Container className="flex items-center gap-4">
              <Link
                icon={false}
                target="_blank"
                href="https://github.com/sptlco"
                className={clsx(
                  "flex items-center justify-center size-10! text-foreground-tertiary!",
                  "bg-button hover:bg-button-hover active:bg-button-active rounded-lg"
                )}
              >
                <LinkedIn className="size-5" />
              </Link>
              <Link
                href="https://github.com/sptlco"
                icon={false}
                target="_blank"
                className={clsx(
                  "flex items-center justify-center size-10! text-foreground-tertiary!",
                  "bg-button hover:bg-button-hover active:bg-button-active rounded-lg"
                )}
              >
                <Github className="size-5" />
              </Link>
            </Container>
          </Container>
        </Container>
        <Container className="flex flex-col justify-between gap-10">
          <Container className="grid gap-10 grid-cols-2 md:grid-cols-3 lg:grid-cols-5">
            {Object.entries(groups).map(([category, links]) => (
              <Container key={category} className="flex flex-col gap-4">
                <Span className="text-sm font-semibold text-foreground-tertiary">{category}</Span>
                <Container className="flex flex-col gap-2">
                  {links.map((link) => (
                    <Link
                      key={link.href}
                      href={link.href}
                      className="text-sm text-foreground-secondary hover:text-foreground-primary transition-colors"
                    >
                      {link.label}
                    </Link>
                  ))}
                </Container>
              </Container>
            ))}
          </Container>
          <Form className="group flex relative w-full gap-4 items-end">
            <Field
              type="email"
              inset={false}
              label="Newsletter"
              placeholder="Enter your email address"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              className="pr-12"
            />
            <Button type="submit" className={clsx("shrink-0! size-6! p-0! absolute right-2 bottom-2", "group-focus-within:bg-button-highlight!")}>
              <Icon symbol="arrow_right_alt" size={20} />
            </Button>
          </Form>
        </Container>
      </Container>
    </footer>
  );
});

const LinkedIn = createElement<typeof Svg>((props, ref) => (
  <Svg {...props} ref={ref} viewBox="0 0 72 72">
    <defs>
      <mask id="linkedin-mask">
        <rect width="72" height="72" fill="white" />
        <Path
          d="M62,62 L51.315625,62 L51.315625,43.8021149 C51.315625,38.8127542 49.4197917,36.0245323 45.4707031,36.0245323 C41.1746094,36.0245323 38.9300781,38.9261103 38.9300781,43.8021149 L38.9300781,62 L28.6333333,62 L28.6333333,27.3333333 L38.9300781,27.3333333 L38.9300781,32.0029283 C38.9300781,32.0029283 42.0260417,26.2742151 49.3825521,26.2742151 C56.7356771,26.2742151 62,30.7644705 62,40.051212 L62,62 Z M16.349349,22.7940133 C12.8420573,22.7940133 10,19.9296567 10,16.3970067 C10,12.8643566 12.8420573,10 16.349,10 C19.8566406,10 22.6970052,12.8643566 22.6970052,16.3970067 C22.6970052,19.9296567 19.8566406,22.7940133 16.349349,22.7940133 Z M11.0325521,62 L21.769401,62 L21.769401,27.3333333 L11.0325521,27.3333333 L11.0325521,62 Z"
          fill="black"
        />
      </mask>
    </defs>
    <Path
      d="M8,72 L64,72 C68.418278,72 72,68.418278 72,64 L72,8 C72,3.581722 68.418278,0 64,0 L8,0 C3.581722,0 0,3.581722 0,8 L0,64 C0,68.418278 3.581722,72 8,72 Z"
      fill="currentColor"
      mask="url(#linkedin-mask)"
    />
  </Svg>
));

const Github = createElement<typeof Svg>((props, ref) => (
  <Svg {...props} ref={ref} viewBox="0 0 1024 1024" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
    <Path
      fillRule="evenodd"
      clipRule="evenodd"
      d="M8 0C3.58 0 0 3.58 0 8C0 11.54 2.29 14.53 5.47 15.59C5.87 15.66 6.02 15.42 6.02 15.21C6.02 15.02 6.01 14.39 6.01 13.72C4 14.09 3.48 13.23 3.32 12.78C3.23 12.55 2.84 11.84 2.5 11.65C2.22 11.5 1.82 11.13 2.49 11.12C3.12 11.11 3.57 11.7 3.72 11.94C4.44 13.15 5.59 12.81 6.05 12.6C6.12 12.08 6.33 11.73 6.56 11.53C4.78 11.33 2.92 10.64 2.92 7.58C2.92 6.71 3.23 5.99 3.74 5.43C3.66 5.23 3.38 4.41 3.82 3.31C3.82 3.31 4.49 3.1 6.02 4.13C6.66 3.95 7.34 3.86 8.02 3.86C8.7 3.86 9.38 3.95 10.02 4.13C11.55 3.09 12.22 3.31 12.22 3.31C12.66 4.41 12.38 5.23 12.3 5.43C12.81 5.99 13.12 6.7 13.12 7.58C13.12 10.65 11.25 11.33 9.47 11.53C9.76 11.78 10.01 12.26 10.01 13.01C10.01 14.08 10 14.94 10 15.21C10 15.42 10.15 15.67 10.55 15.59C13.71 14.53 16 11.53 16 8C16 3.58 12.42 0 8 0Z"
      transform="scale(64)"
    />
  </Svg>
));

// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { clsx } from "clsx";
import { useState } from "react";

import { Button, Container, createElement, Facebook, Icon, Link, LinkedIn, Span, UL, X } from "@sptlco/design";

/**
 * Allows the user to share a post to social media.
 */
export const Share = createElement<typeof Container>((props, ref) => {
  const [copied, setCopied] = useState(false);

  const url = typeof window !== "undefined" ? window.location.href : "";
  const encoded = encodeURIComponent(url);

  const links = {
    x: `https://twitter.com/intent/tweet?url=${encoded}`,
    linkedin: `https://www.linkedin.com/sharing/share-offsite/?url=${encoded}`,
    facebook: `https://www.facebook.com/sharer/sharer.php?u=${encoded}`
  };

  const handleCopy = async () => {
    try {
      await navigator.clipboard.writeText(url);

      setCopied(true);
      setTimeout(() => setCopied(false), 2000);
    } catch {
      const input = document.createElement("input");
      input.value = url;
      document.body.appendChild(input);
      input.select();
      document.execCommand("copy");
      document.body.removeChild(input);
      setCopied(true);
      setTimeout(() => setCopied(false), 2000);
    }
  };

  return (
    <Container {...props} ref={ref} className={clsx("flex flex-col gap-5", props.className)}>
      <Span className="text-foreground-secondary">Share</Span>
      <UL className="flex w-full items-center gap-5">
        <Link
          icon={false}
          href={links.x}
          target="_blank"
          rel="noopener noreferrer"
          className="inline-flex items-center justify-center size-5 text-foreground-tertiary hover:text-foreground-primary"
        >
          <X className="h-full" />
        </Link>
        <Link
          icon={false}
          href={links.linkedin}
          target="_blank"
          rel="noopener noreferrer"
          className="inline-flex items-center justify-center size-5 text-foreground-tertiary hover:text-foreground-primary"
        >
          <LinkedIn className="w-full" />
        </Link>
        <Link
          icon={false}
          href={links.facebook}
          target="_blank"
          rel="noopener noreferrer"
          className="inline-flex items-center justify-center size-5 text-foreground-tertiary hover:text-foreground-primary"
        >
          <Facebook className="h-full" />
        </Link>
        <Button
          intent="none"
          size="fit"
          className="inline-flex size-5 text-foreground-tertiary hover:text-foreground-primary"
          onClick={handleCopy}
          aria-label={copied ? "Copied!" : "Copy link"}
        >
          <Icon symbol={copied ? "check" : "content_copy"} size={20} className="font-medium" />
        </Button>
      </UL>
    </Container>
  );
});

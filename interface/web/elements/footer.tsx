// Copyright © Spatial Corporation. All rights reserved.

import { createElement, Container, Link } from "@sptlco/design";
import { clsx } from "clsx";
import { useTranslations } from "next-intl";

/**
 * A minimal footer used for interactive pages.
 */
export const CompactFooter = createElement<typeof Container>((props, ref) => {
  const t = useTranslations("footer");

  return (
    <Container
      {...props}
      ref={ref}
      className={clsx("col-span-3 row-start-3 place-self-center flex flex-col sm:flex-row items-center gap-4 sm:gap-10 text-sm", props.className)}
    >
      <Link className="text-foreground-tertiary" target="_blank">
        {t("legal.users")}
      </Link>
      <Link className="text-foreground-tertiary" target="_blank">
        {t("legal.privacy")}
      </Link>
    </Container>
  );
});

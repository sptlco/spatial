// Copyright © Spatial Corporation. All rights reserved.

import { Button, Container, createElement, Dialog, Icon, Link, Logo } from "@sptlco/design";

type Hyperlink = {
  href: string;
  label: string;
};

export const Navigation = createElement<"nav">((props, ref) => {
  const links: Hyperlink[] = [];

  return (
    <nav {...props} ref={ref} className="flex flex-col items-center">
      <Container className="flex items-center justify-between gap-10 p-10 w-full xl:max-w-7xl">
        <Link href="/">
          <Logo mode="symbol" className="h-4 md:h-6" />
        </Link>
        <Container className="hidden lg:flex items-center gap-10">
          {links.map((link, i) => (
            <Link key={i} href={link.href} className="text-sm">
              {link.label}
            </Link>
          ))}
        </Container>
        <Container className="ml-auto flex items-center gap-2.5">
          <Dialog.Root>
            <Dialog.Trigger asChild>
              <Button intent="none" className="p-0! size-10!">
                <Icon symbol="search" />
              </Button>
            </Dialog.Trigger>
          </Dialog.Root>
          <Dialog.Root>
            <Dialog.Trigger asChild>
              <Button intent="none" className="p-0! size-10!">
                <Icon symbol="local_mall" />
              </Button>
            </Dialog.Trigger>
          </Dialog.Root>
        </Container>
      </Container>
    </nav>
  );
});

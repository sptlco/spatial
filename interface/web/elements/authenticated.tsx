// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { useRouter } from "@/locales/navigation";
import { useUser } from "@/stores";
import { Container, Spinner } from "@sptlco/design";
import { clsx } from "clsx";
import { FC, PropsWithChildren, useEffect } from "react";
import { useShallow } from "zustand/shallow";

/**
 * Configurable options for an authenticated component.
 */
export type AuthenticatedProps = PropsWithChildren & {
  /**
   * Whether or not to display the loading animation.
   */
  animate?: boolean;

  /**
   * Where to redirect unauthenticated uers.
   */
  redirect: string;
};

/**
 * A component that's only rendered if the user is authenticated.
 * @param props Configurable options for the component.
 */
export const Authenticated: FC<AuthenticatedProps> = ({ animate = true, ...props }) => {
  const router = useRouter();

  const { loading, authenticated } = useUser(
    useShallow((state) => ({
      loading: state.loading,
      authenticated: state.authenticated
    }))
  );

  useEffect(() => {
    if (!loading && !authenticated) {
      router.push(props.redirect);
    }
  }, [loading, authenticated]);

  return (
    <>
      {animate && (
        <Container
          className={clsx(
            "absolute inset-0 size-full z-50 bg-background-base/30 backdrop-blur flex items-center justify-center",
            { "animate-in fade-in": loading || !authenticated },
            { "animate-out fade-out fill-mode-forwards pointer-events-none": !loading && authenticated },
            "duration-500"
          )}
        >
          <Spinner className="size-6" />
        </Container>
      )}
      {props.children}
    </>
  );
};

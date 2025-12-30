// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { useRouter } from "@/locales/navigation";
import { User, useUser } from "@/stores";
import { Container, Spinner } from "@sptlco/design";
import { clsx } from "clsx";
import { FC, PropsWithChildren, useEffect } from "react";
import { useShallow } from "zustand/shallow";

/**
 * Configurable options for an authenticated component.
 */
export type AuthenticatedProps = PropsWithChildren & {
  /**
   * An optional condition under which to allow an authenticated user to access the component.
   */
  condition?: (user: User) => boolean;
};

/**
 * A component that requires an authenticated user.
 * @param props Configurable options for the component.
 */
export const Guard: FC<AuthenticatedProps> = (props) => {
  const router = useRouter();

  const { user, loading, authenticated } = useUser(
    useShallow((state) => ({
      user: state,
      loading: state.loading,
      authenticated: state.authenticated
    }))
  );

  useEffect(() => {
    if (!loading) {
      if (!authenticated || (props.condition && !props.condition(user))) {
        router.push(`/session/new?next=${btoa(window.location.href)}`);
      }
    }
  }, [loading, authenticated]);

  return (
    <Container
      className={clsx(
        "fixed inset-0 w-screen h-screen z-50 bg-background-base/30 backdrop-blur flex items-center justify-center",
        { "animate-in fade-in": loading || !authenticated },
        { "animate-out fade-out fill-mode-forwards pointer-events-none": !loading && authenticated },
        "duration-500"
      )}
    >
      <Spinner className="size-6" />
    </Container>
  );
};

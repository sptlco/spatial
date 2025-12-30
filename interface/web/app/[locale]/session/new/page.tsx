// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { CompactFooter } from "@/elements";
import { useRouter } from "@/locales/navigation";
import { LocaleSwitcher } from "@/locales/switch";
import { useUser } from "@/stores";
import { SESSION_COOKIE_NAME, Spatial } from "@sptlco/client";
import { Button, Container, Dialog, Field, Form, H1, Icon, Link, Logo, Main, OTP, Paragraph, Span, Spinner, toast } from "@sptlco/design";
import { clsx } from "clsx";
import cookies from "js-cookie";
import { useTranslations } from "next-intl";
import { useSearchParams } from "next/navigation";
import { FC, FormEvent, Suspense, useEffect, useState } from "react";
import { useShallow } from "zustand/shallow";

const KEY_LENGTH = 4;

type AuthenticationState = "idle" | "requesting" | "confirming" | "verifying" | "authenticated";

/**
 * A page that authenticates the current user.
 * @returns A user authentication page.
 */
export default function Page() {
  const { authenticated, loading, login } = useUser(
    useShallow((state) => ({
      authenticated: state.authenticated,
      loading: state.loading,
      login: state.login
    }))
  );

  const t = useTranslations("authentication");

  const [state, setState] = useState<AuthenticationState>("idle");
  const [email, setEmail] = useState("");
  const [code, setCode] = useState("");

  const processing = state === "requesting" || state === "verifying";
  const open = state === "confirming" || state === "verifying";

  const request = async (e: FormEvent) => {
    e.preventDefault();

    if (state !== "idle") {
      return;
    }

    setState("requesting");

    const response = await Spatial.keys.create({ user: email });

    if (response.error) {
      toast.error(t("errors.request.title"), {
        closeButton: true,
        description: t("errors.request.description")
      });

      setState("idle");
      return;
    }

    setState("confirming");
  };

  const verify = async () => {
    if (state !== "confirming") {
      return;
    }

    setState("verifying");

    const response = await Spatial.sessions.create({ user: email, key: code });

    if (response.error) {
      toast.error(t("errors.verify.title"), {
        description: t("errors.verify.description")
      });

      setCode("");
      setState("confirming");

      return;
    }

    cookies.set(SESSION_COOKIE_NAME, response.data.token, {
      path: "/",
      secure: true,
      expires: new Date(response.data.expires)
    });

    setState("authenticated");

    await login();
  };

  const Redirect: FC = () => {
    const router = useRouter();
    const searchParams = useSearchParams();

    const next = () => {
      let href = "/";
      let param = searchParams.get("next");

      if (param) {
        href = atob(param);
      }

      return href;
    };

    useEffect(() => {
      if (!loading && authenticated) {
        router.push(next());
      }
    }, [loading, authenticated]);

    return null;
  };

  return (
    <Main className="grid grid-cols-[1fr_auto_1fr] grid-rows-[auto_1fr_auto] p-10 gap-y-20 w-full min-h-screen">
      <Container
        className={clsx(
          "fixed inset-0 w-screen h-screen z-50 bg-background-base/30 backdrop-blur flex items-center justify-center",
          { "animate-in fade-in": loading || authenticated },
          { "animate-out fade-out fill-mode-forwards pointer-events-none": !loading && !authenticated },
          "duration-500"
        )}
      >
        <Spinner className="size-6" />
      </Container>
      <Container className="row-start-1 col-start-2 sm:col-start-3 sm:justify-self-end h-10 flex items-center justify-center">
        <LocaleSwitcher />
      </Container>
      <Form
        className="row-start-2 col-span-full place-self-center flex flex-col items-center justify-center w-full sm:max-w-sm space-y-10"
        onSubmit={request}
      >
        <Container className="flex flex-col w-full items-center space-y-10">
          <Logo className="fill-current h-8" mode="symbol" />
          <Container className="w-full flex flex-col text-center">
            <H1 className="font-medium">{t("title")}</H1>
            <Paragraph className="text-foreground-secondary">{t("description")}</Paragraph>
          </Container>
        </Container>
        <Field
          type="text"
          label={t("email.label")}
          name="email"
          id="email"
          placeholder={t("email.placeholder")}
          description={t.rich("email.description", {
            link: (chunks) => (
              <Link href="/docs" target="_blank">
                {chunks}
              </Link>
            )
          })}
          disabled={state !== "idle"}
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
        <Button type="submit" className="w-full" intent="secondary">
          <Span>{t("continue")}</Span>
          {processing ? <Spinner className="size-4 m-1 text-foreground-tertiary" /> : <Icon symbol="arrow_right_alt" />}
        </Button>
      </Form>
      <CompactFooter />
      <Dialog.Root
        open={open}
        onOpenChange={(open) => {
          if (!open) {
            setState("idle");
            setCode("");
          }
        }}
      >
        <Dialog.Content
          title={t("verification.title")}
          description={<>{t("verification.description")}</>}
          className="flex flex-col w-full sm:max-w-sm items-center space-y-10"
        >
          <Logo className="fill-current h-8" mode="symbol" />
          <Field
            type="otp"
            maxLength={KEY_LENGTH}
            label={t("verification.label")}
            value={code}
            onValueChange={(value) => setCode(value.toUpperCase())}
            onComplete={verify}
            disabled={state === "verifying" || state === "authenticated"}
            autoFocus
            description={t.rich("verification.help", { link: (chunks) => <Link>{chunks}</Link> })}
            containerClassName="items-center text-center"
            className="justify-center"
          >
            <OTP.Group className="justify-center">
              {[...Array(KEY_LENGTH)].map((_, index) => (
                <OTP.Slot key={index} index={index} />
              ))}
            </OTP.Group>
          </Field>
          <Container className="inline-flex w-full h-6 items-center justify-center">
            {processing && <Spinner className="size-6 text-foreground-tertiary" />}
          </Container>
        </Dialog.Content>
      </Dialog.Root>
      <Suspense>
        <Redirect />
      </Suspense>
    </Main>
  );
}

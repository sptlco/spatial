// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Unauthenticated } from "@/elements";
import { LocaleSwitcher } from "@/locales/switch";
import { useUser } from "@/stores";
import { SESSION_COOKIE_NAME, Spatial } from "@sptlco/client";
import { Button, Container, Dialog, Field, Form, H1, Icon, Link, Logo, Main, OTP, Paragraph, resolve, Span, Spinner } from "@sptlco/design";
import cookies from "js-cookie";
import { useTranslations } from "next-intl";
import { useSearchParams } from "next/navigation";
import { FormEvent, Suspense, useState } from "react";

const KEY_LENGTH = 4;

type AuthenticationState = "idle" | "requesting" | "confirming" | "verifying" | "authenticated";

/**
 * A page that authenticates the current user.
 * @returns A user authentication page.
 */
export default function Page() {
  return (
    <Suspense>
      <Authentication />
    </Suspense>
  );
}

const Authentication = () => {
  const authenticate = useUser((state) => state.authenticate);

  const searchParams = useSearchParams();
  const t = useTranslations("authentication");

  const [state, setState] = useState<AuthenticationState>("idle");
  const [email, setEmail] = useState("");
  const [code, setCode] = useState("");

  const processing = state === "requesting" || state === "verifying";
  const open = state === "confirming" || state === "verifying";

  const next = () => {
    let href = resolve("/");
    let param = searchParams.get("next");

    if (param) {
      href = atob(param);
    }

    return href;
  };

  const request = async (e: FormEvent) => {
    e.preventDefault();

    if (state !== "idle") {
      return;
    }

    setState("requesting");

    const response = await Spatial.keys.create({ user: email });

    if (response.error) {
      // ...

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
      // ...

      setCode("");
      setState("confirming");
      return;
    }

    cookies.set(SESSION_COOKIE_NAME, response.data.token, {
      path: "/",
      secure: true,
      expires: 365
    });

    await authenticate();

    setState("authenticated");
  };

  const render = () => {
    return (
      <>
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
        <Container className="col-span-3 row-start-3 place-self-center flex flex-col sm:flex-row items-center gap-4 sm:gap-10 text-sm">
          <Link className="text-foreground-tertiary" target="_blank">
            {t("legal.users")}
          </Link>
          <Link className="text-foreground-tertiary" target="_blank">
            {t("legal.privacy")}
          </Link>
        </Container>
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
      </>
    );
  };

  return (
    <Unauthenticated redirect={next()}>
      <Main className="grid grid-cols-[1fr_auto_1fr] grid-rows-[auto_1fr_auto] p-10 gap-y-20 w-full min-h-screen">{render()}</Main>
    </Unauthenticated>
  );
};

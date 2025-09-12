// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { useEffect, useRef } from "react";
import * as THREE from "three";

import { Div, Element, ElementProps, Node } from "..";

/**
 * Create a new universe element.
 * @param props Configurable options for the element.
 * @returns A universe element.
 */
export const Universe: Element = (props: ElementProps): Node => {
  const mountRef = useRef<HTMLDivElement | null>(null);

  useEffect(() => {
    const scene = new THREE.Scene();
    const camera = new THREE.PerspectiveCamera(
      75,
      window.innerWidth / window.innerHeight,
      0.1,
      1000,
    );

    const renderer = new THREE.WebGLRenderer();

    renderer.setSize(window.innerWidth, window.innerHeight);
    mountRef.current?.appendChild(renderer.domElement);

    const starsGeometry = new THREE.BufferGeometry();
    const starsAmount = 1000;
    const positions = new Float32Array(starsAmount * 3);

    for (let i = 0; i < starsAmount * 3; i++) {
      positions[i] = (Math.random() - 0.5) * 2000;
    }

    starsGeometry.setAttribute(
      "position",
      new THREE.BufferAttribute(positions, 3),
    );

    const starsMaterial = new THREE.PointsMaterial({
      color: 0xffffff,
      size: 1,
      sizeAttenuation: false,
    });

    const stars = new THREE.Points(starsGeometry, starsMaterial);
    scene.add(stars);

    camera.position.z = 500;

    const animate = () => {
      requestAnimationFrame(animate);
      stars.rotation.y += 0.001;
      renderer.render(scene, camera);
    };

    animate();

    return () => {
      mountRef.current?.removeChild(renderer.domElement);
      renderer.dispose();
    };
  }, []);

  return (
    <Div
      className={props.className}
      ref={mountRef}
      style={{ width: "100%", height: "100%" }}
    />
  );
};

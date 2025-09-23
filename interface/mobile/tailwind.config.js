/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./App.tsx", "./components/**/*.{js,jsx,ts,tsx}"],
  presets: [require("nativewind/preset")],
  plugins: [],
  theme: {
    extend: {
      colors: {
        background: "#000000"
      }
    }
  }
}
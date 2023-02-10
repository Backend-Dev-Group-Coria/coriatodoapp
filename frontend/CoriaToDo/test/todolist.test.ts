import { describe, expect, it } from 'vitest'
import { $fetch } from '@nuxt/test-utils'

describe('Renders List of Items', () => {
    it('', async () => {
        expect(await $fetch('/')).toMatch('Hello Nuxt!')
    })
})
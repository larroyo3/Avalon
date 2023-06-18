import { describe, it, expect } from 'vitest'

import { mount } from '@vue/test-utils'
import HelloWorld from '../HelloWorld.vue'
import TheWelcome from '../TheWelcome.vue'
import App from '../../App.vue'
import AccountView from '../../views/AccountView.vue'
import CreateView from '../../views/CreateView.vue'

describe('HelloWorld', () => {
  it('renders properly', () => {
    const wrapper = mount(HelloWorld, { props: { msg: 'Hello Vitest' } })
    expect(wrapper.text()).toContain('Hello Vitest')
  })
})

describe('The welcome', () => {
  it('renders properly', () => {
    const wrapper = mount(TheWelcome)
    expect(wrapper.text()).toContain('Documentation')
  })
})


describe('App', () => {
  it('renders correctly', () => {
    const wrapper = mount(App);
    
    expect(wrapper.find('v-dialog').exists()).toBe(true);
    expect(wrapper.find('v-card').exists()).toBe(true);
  });
});

describe('AccountView', () => {
  it('renders properly', () => {
    const wrapper = mount(AccountView)
    
    expect(wrapper.find('v-card').exists()).toBe(true)
    expect(wrapper.find('v-text-field[label="Name*"]').exists()).toBe(true)
    expect(wrapper.find('v-text-field[label="Password*"]').exists()).toBe(true)
    expect(wrapper.find('v-select[label="Package*"]').exists()).toBe(true)
  })
})




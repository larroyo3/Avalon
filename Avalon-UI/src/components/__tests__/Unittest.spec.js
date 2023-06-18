import { shallowMount } from '@vue/test-utils';
import { mount } from '@vue/test-utils';
import { describe, it, expect } from 'vitest'
import Account from '@/views/AccountView.vue';
import App from '../../App.vue'

describe('Account.vue', () => {
  it('should update the user and call updateUser method', () => {
    const wrapper = shallowMount(Account);
    wrapper.vm.package = 'Pro (10 daily upload)';

    wrapper.vm.saveChange();

    expect(wrapper.vm.remainingPhoto).toBe(10);
  });
});

describe('App.vue', () => {
  it('initializes with correct data', () => {
    const wrapper = shallowMount(App);
    expect(wrapper.vm.dialog).toBe(false);
    expect(wrapper.vm.tab).toBe(null);
    expect(wrapper.vm.userId).toBe('0');
    expect(wrapper.vm.snackbar).toBe(false);
    expect(wrapper.vm.text).toBe('Error during log in');
    expect(wrapper.vm.timeout).toBe(3000);
    expect(wrapper.vm.valid).toBe(false);
    expect(wrapper.vm.name).toBe('');
    expect(wrapper.vm.password).toBe('');
    expect(wrapper.vm.remainingPhoto).toBe(3);
    expect(wrapper.vm.profilePhoto).toBeNull();
    expect(wrapper.vm.package).toBe('Free (3 daily upload)');
    expect(wrapper.vm.packages).toEqual([
      'Free (3 daily upload)',
      'Pro (10 daily upload)',
      'Gold (10000 daily upload)'
    ]);
  });
});

describe('App.vue', () => {
  it('disables the button when name or password is empty', () => {
    const wrapper = shallowMount(App);
    expect(wrapper.vm.isDisabled).toBe(true);

    wrapper.setData({ name: 'John Doe', password: 'password123' });
    expect(wrapper.vm.isDisabled).toBe(false);
  });
});